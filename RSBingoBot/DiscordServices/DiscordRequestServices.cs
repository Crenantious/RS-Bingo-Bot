// <copyright file="DiscordRequestServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordServices;

using RSBingoBot.Requests;
using Microsoft.Extensions.Logging;
using DSharpPlus.Entities;

internal class DiscordRequestServices
{
    private const string RequestCompletedSuccessfullyResponse = "Request completed successfully.";
    private const string ProcessingRequestResponse = "Processing request.";
    private const string UnknownErrorResponse = "An unknown error occurred while processing this command.";

    private static readonly ILogger<DiscordRequestServices> logger;
    private static readonly DiscordInteractionServices interactionServices;

    static DiscordRequestServices()
    {
        interactionServices = (DiscordInteractionServices)General.DI.GetService(typeof(DiscordInteractionServices))!;
        logger = General.LoggingInstance<DiscordRequestServices>();
    }

    /// <summary>
    /// Attempts to send a request to Discord. If the request fails through a web error it will be retried a number of times.
    /// </summary>
    /// <returns><see langword="true"/> if the request was successful, <see langword="false"/> otherwise.</returns>
    public static async Task<bool> SendDiscordRequest(Func<Task> request)
    {
        // TODO: JR - make this auto retry if there was a non fatal error (i.e. a web exception).
        // Return false if the request is invalid (i.e. "invalid body form" in a message).
        try { await request.Invoke(); }
        catch { return false; }
        return true;
    }

    public async Task RunRequest(DiscordInteraction interaction, RequestBase request)
    {
        IEnumerable<string> responses = Enumerable.Empty<string>();

        try
        {
            if (await SendKeepAliveMessage(interaction) is false)
            {
                // Could not establish a connection to the discord channel. So we do not attempt to set a message for a response. 
                // As the response when sent would throw.
                return;
            }

            responses = (await request.Run()).Responses;
        }
        catch (Exception e)
        {
            responses = new List<string> { UnknownErrorResponse };
            logger.LogError(e, e.Data.ToString());
            return;
        }
        finally
        {
            if (responses.Any() is false)
            {
                responses = new string[] { RequestCompletedSuccessfullyResponse };
            }

            await interactionServices.DeleteOriginalResponse(interaction);
            await SendResponseMessages(interaction, responses);
        }
    }

    private static async Task SendResponseMessages(DiscordInteraction interaction, IEnumerable<string> responses)
    {
        DiscordFollowupMessageBuilder builder = new() { IsEphemeral = true };

        foreach (string message in responses)
        {
            builder.WithContent(message);
            await interactionServices.Followup(interaction, builder);
        }
    }

    private static async Task<bool> SendKeepAliveMessage(DiscordInteraction interaction) =>
        await interactionServices.Respond(interaction, ProcessingRequestResponse, true);
}