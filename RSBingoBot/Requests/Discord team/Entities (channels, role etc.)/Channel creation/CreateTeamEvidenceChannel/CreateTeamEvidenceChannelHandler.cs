// <copyright file="CreateTeamEvidenceChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamEvidenceChannelHandler : RequestHandler<CreateTeamEvidenceChannelRequest>
{
    private const string ChannelName = "{0}-evidence";

    private readonly IDiscordServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamEvidenceChannelHandler(IDiscordServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamEvidenceChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.DiscordTeam.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetCategory(request.DiscordTeam.Role!);

        Result<DiscordChannel> channel = await channelServices.CreateChannel(
            name, DSharpPlus.ChannelType.Text, request.DiscordTeam.CategoryChannel, overwrites);

        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetEvidenceChannel(channel.Value);
            AddSuccess(new CreateTeamGeneralChannelSuccess());
        }
        else
        {
            AddError(new CreateTeamGeneralChannelError());
        }
    }
}