// <copyright file="CreateTeamGeneralChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.DTO;
using RSBingoBot.Requests;

internal class CreateTeamGeneralChannelHandler : RequestHandler<CreateTeamGeneralChannelRequest, Result>
{
    private const string ChannelName = "{0}-general";

    private readonly DiscordChannelServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamGeneralChannelHandler(DiscordChannelServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamGeneralChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetCategory(request.TeamRole);

        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Text, request.Category, overwrites);
        if (channel is null)
        {
            AddError(new CreateTeamGeneralChannelError());
        }
        else
        {
            request.Team.GeneralChannelId = channel.Id;
            AddSuccess(new CreateTeamGeneralChannelSuccess());
        }
    }
}