// <copyright file="CreateTeamBoardChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamBoardChannelHandler : RequestHandler<CreateTeamBoardChannelRequest, Result>
{
    private const string ChannelName = "{0}-board";

    private readonly DiscordChannelServices channelServices;

    public CreateTeamBoardChannelHandler(DiscordChannelServices channelServices)
    {
        this.channelServices = channelServices;
    }

    protected override async Task Process(CreateTeamBoardChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Category);
        if (channel is null)
        {
            AddError(new CreateTeamBoardChannelError());
        }
        else
        {
            request.Team.CategoryChannelId = channel.Id;
            AddSuccess(new CreateTeamBoardChannelSuccess());
        }
    }
}