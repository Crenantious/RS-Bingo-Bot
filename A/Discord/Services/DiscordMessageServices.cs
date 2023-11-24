// <copyright file="DiscordMessageServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System.Text;

// TODO: JR - convert to using the requests system.
public class DiscordMessageServices : IDiscordMessageServices
{
    private readonly Logger<DiscordChannelServices> logger;

    private DiscordMessageServices(Logger<DiscordChannelServices> logger)
    {
        this.logger = logger;
    }

    private enum RequestType
    {
        Sent,
    }

    public async Task<bool> Send(Message message, DiscordChannel channel)
    {
        var request = () => channel.SendMessageAsync(message.GetMessageBuilder());
        return await SendRequest(request, message, channel, RequestType.Sent);
    }

    public async Task<Message> Get(ulong id, DiscordChannel channel)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> SendRequest(Func<Task> request, Message message, DiscordChannel channel, RequestType requestType)
    {
        try
        {
            await request();
            Log(true, message, channel, requestType);
            return true;
        }
        catch
        {
            Log(false, message, channel, requestType);
            return false;
        }
    }

    private void Log(bool wasSuccessful, Message message, DiscordChannel channel, RequestType requestType)
    {
        string outcome = wasSuccessful ? "Successfully" : "Unsuccessfully";
        string information = $"{outcome} {requestType} a message to a channel. " +
            $"Channel name: {channel.Name}, channel id: {channel.Id}. Message content: {message.Content}, " +
            $"Components: {GetComponentNames(message)}.";

        if (wasSuccessful)
        {
            logger.LogInformation(information);
            return;
        }

        logger.LogError(information);
    }

    private string GetComponentNames(Message message)
    {
        StringBuilder sb = new();

        foreach (List<IComponent> rows in message.Components.GetRows())
        {
            StringBuilder row = new("(");
            foreach (IComponent component in rows)
            {
                row.AppendJoin(", ", rows.Select(c => c.Name));
            }
            row.Append(")");

            // TODO: JR - see if this appends with the comma or if that's just used for the elements passed in.
            sb.AppendJoin(", ", row);
        }

        return sb.ToString();
    }

    private string GetImageNames(Message message)
    {
        // TODO: JR - implement then add to Log.
        throw new NotImplementedException();
    }
}