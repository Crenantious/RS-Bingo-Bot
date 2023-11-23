// <copyright file="DiscordChannelServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;

public class DiscordChannelServices : IDiscordChannelServices
{
    private readonly Logger<DiscordChannelServices> logger;

    private DiscordChannelServices(Logger<DiscordChannelServices> logger)
    {
        this.logger = logger;
    }

    private enum RequestType
    {
        Created,
    }

    public async Task<DiscordChannel?> Create(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null)
    {
        var request = () => DataFactory.Guild.CreateChannelAsync(name, channelType, parent!, overwrites: overwrites!);
        (bool success, DiscordChannel? channel) = await SendRequest(request!, name, RequestType.Created);
        return channel;
    }

    private async Task<(bool, DiscordChannel?)> SendRequest(Func<Task<DiscordChannel?>> request, string name, RequestType requestType)
    {
        try
        {
            DiscordChannel? channel = await request();
            Log(name, requestType, true);
            return (true, channel);
        }
        catch
        {
            Log(name, requestType, false);
            return (false, null);
        }
    }

    private void Log(string name, RequestType requestType, bool wasSuccessful)
    {
        // TODO: JR - check if this information is sufficient.
        string outcome = wasSuccessful ? "Successfully" : "Unsuccessfully";
        string information = $"{requestType} a channel named '{name}' {outcome}. ";

        if (wasSuccessful)
        {
            logger.LogInformation(information);
            return;
        }

        logger.LogError(information);
    }
}