﻿// <copyright file="CreateTeamBoardChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.DTO;
using RSBingoBot.Requests;

internal class CreateTeamBoardChannelHandler : RequestHandler<CreateTeamBoardChannelRequest, Result>
{
    private const string ChannelName = "{0}-board";

    private readonly DiscordChannelServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamBoardChannelHandler(DiscordChannelServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamBoardChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetCategory(request.TeamRole);

        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Text, request.Category, overwrites);
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