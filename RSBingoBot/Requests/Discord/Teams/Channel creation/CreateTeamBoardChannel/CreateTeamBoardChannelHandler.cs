// <copyright file="CreateTeamBoardChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamBoardChannelHandler : RequestHandler<CreateTeamBoardChannelRequest>
{
    private const string ChannelName = "{0}-board";

    private readonly IDiscordServices services;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamBoardChannelHandler(IDiscordServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.services = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamBoardChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.DiscordTeam.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetBoard(request.DiscordTeam.Role!);

        Result<DiscordChannel> channel = await services.CreateChannel(
            name, DSharpPlus.ChannelType.Text, request.DiscordTeam.CategoryChannel, overwrites);

        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetBoardChannel(channel.Value);
            AddSuccess(new CreateTeamBoardChannelSuccess());
        }
        else
        {
            AddError(new CreateTeamBoardChannelError());
        }
    }
}