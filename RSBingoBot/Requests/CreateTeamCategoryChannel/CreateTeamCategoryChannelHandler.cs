// <copyright file="CreateTeamCategoryChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamCategoryChannelHandler : RequestHandler<CreateTeamCategoryChannelRequest, Result>
{
    private const string ChannelName = "{0}";

    private readonly DiscordChannelServices channelServices;

    public CreateTeamCategoryChannelHandler(DiscordChannelServices channelServices)
    {
        this.channelServices = channelServices;
    }

    protected override async Task Process(CreateTeamCategoryChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Category);
        if (channel is null)
        {
            AddError(new CreateTeamCategoryChannelError());
        }
        else
        {
            request.Team.CategoryChannelId = channel.Id;
            AddSuccess(new CreateTeamCategoryChannelSuccess());
        }
    }
}