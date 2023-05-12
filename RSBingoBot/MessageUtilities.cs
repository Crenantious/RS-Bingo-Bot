// <copyright file="MessageUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot;

using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using static RSBingo_Common.General;

public static class MessageUtilities
{
    public static async Task Respond(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordInteractionResponseBuilder()
            .WithContent(content)
            .AsEphemeral();

        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, builder);
    }

    public static async Task EditResponse(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordWebhookBuilder()
            .WithContent(content);

        await args.Interaction.EditOriginalResponseAsync(builder);
    }

    public static async Task Followup(InteractionCreateEventArgs args, string content, bool isEphemeral)
    {
        var builder = new DiscordFollowupMessageBuilder()
            .WithContent(content)
            .AsEphemeral();

        await args.Interaction.CreateFollowupMessageAsync(builder);
    }

    public static async Task EditFollowup(InteractionCreateEventArgs args, ulong messageId, string content, bool isEphemeral)
    {
        var builder = new DiscordWebhookBuilder()
            .WithContent(content);

        await args.Interaction.EditFollowupMessageAsync(messageId, builder);
    }

    /// <summary>
    /// <paramref name="messages"/> are added in turn to a compiled message. If adding the message would cause the compiled message
    /// to exceed <paramref name="maxMessageChars"/> in length, a new compiled message is created with that message instead.
    /// </summary>
    /// <param name="messages">The messages to be compiled.</param>
    /// <param name="maxMessageChars">The maximum amount of characters each compiled message can contain.</param>
    /// <returns>The compiled messages.</returns>
    public static IEnumerable<string> GetCompiledMessages(IEnumerable<string> messages, int maxMessageChars = MaxCharsPerDiscordMessage)
    {
        List<string> compiledMessages = new();

        if (messages.Any() is false) { return compiledMessages; }

        while (messages.Any())
        {
            (string compiledMessage, messages) = GetFirstCompiledMessage(messages, maxMessageChars);
            compiledMessages.Add(compiledMessage);
        }

        return compiledMessages;
    }

    private static (string message, IEnumerable<string> reminingWarnings) GetFirstCompiledMessage(IEnumerable<string> messages, int maxMessageChars)
    {
        StringBuilder compiledMessage = new();
        int messageIndex = 0;

        foreach (string message in messages)
        {
            if (compiledMessage.Length + message.Length == maxMessageChars)
            {
                compiledMessage.Append(message);
                messageIndex++;
                break;
            }

            if (compiledMessage.Length + message.Length + Environment.NewLine.Length > maxMessageChars)
            {
                if (messageIndex == 0)
                {
                    compiledMessage.Append(message.Substring(0, maxMessageChars));
                    string[] remainingMessages = messages.ToArray();
                    remainingMessages[0] = remainingMessages[0].Substring(maxMessageChars);
                    messages = remainingMessages;
                }
                break;
            }

            if (messageIndex == 0) { compiledMessage.Append(message); }
            else { compiledMessage.Append(Environment.NewLine + message); }

            messageIndex++;
        }

        return (compiledMessage.ToString(), messages.Skip(messageIndex));
    }
}