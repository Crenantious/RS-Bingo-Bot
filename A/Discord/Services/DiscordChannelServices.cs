// <copyright file="DiscordChannelServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using RSBingo_Framework.DAL;

// TODO: JR - convert to using the requests system.
public class DiscordChannelServices : IDiscordChannelServices
{
    private readonly Logger<DiscordChannelServices> logger;

    private DiscordChannelServices(Logger<DiscordChannelServices> logger)
    {
        this.logger = logger;
    }

    public async Task<DiscordChannel?> Create(string name, ChannelType channelType, DiscordChannel? parent = null,
        IEnumerable<DiscordOverwriteBuilder>? overwrites = null)
    {
        var request = () => DataFactory.Guild.CreateChannelAsync(name, channelType, parent!, overwrites: overwrites!);
        string beginMessage = $"Attempting to create channel with name {name}.";
        var successMessage = (DiscordChannel channel) => $"Successfully created channel with name '{channel.Name}' and id {channel.Id}.";
        string failureMessage = $"Failed to create channel with name {name}.";
        (bool success, DiscordChannel? channel) = await SendRequestAsync(request, beginMessage, successMessage, failureMessage);
        return channel;
    }

    public DiscordChannel? Get(ulong id)
    {
        var request = () => DataFactory.Guild.GetChannel(id);
        string beginMessage = $"Attempting to retrieve channel with id {id}.";
        var successMessage = (DiscordChannel channel) => $"Successfully retrieved channel '{channel.Name}' with id {id}.";
        string failureMessage = $"Failed to retrieve channel with id {id}.";
        (bool success, DiscordChannel? channel) = SendRequest<DiscordChannel>(request, beginMessage, successMessage, failureMessage);
        return channel;
    }

    private (bool, T?) SendRequest<T>(Func<T> request, string beginMessage,
        Func<T, string> successMessage, string failureMessage)
    {
        try
        {
            logger.LogInformation(beginMessage);
            T result = request();
            logger.LogInformation(successMessage(result));
            return (true, result);
        }
        catch
        {
            logger.LogError(failureMessage);
            return (false, default);
        }
    }

    private async Task<(bool, T?)> SendRequestAsync<T>(Func<Task<T>> request, string beginMessage,
        Func<T, string> successMessage, string failureMessage)
    {
        try
        {
            logger.LogInformation(beginMessage);
            T? result = await request();
            logger.LogInformation(successMessage(result));
            return (true, result);
        }
        catch
        {
            logger.LogError(failureMessage);
            return (false, default);
        }
    }
}