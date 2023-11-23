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

    public async Task<bool> Create(string name, ChannelType channelType, DiscordChannel? parent = null) =>
        await SendRequest(() => DataFactory.Guild.CreateChannelAsync(name, channelType, parent!), name, RequestType.Created);

    private async Task<bool> SendRequest(Func<Task> request, string name, RequestType requestType)
    {
        try
        {
            await request();
            Log(name, requestType, true);
            return true;
        }
        catch
        {
            Log(name, requestType, false);
            return false;
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