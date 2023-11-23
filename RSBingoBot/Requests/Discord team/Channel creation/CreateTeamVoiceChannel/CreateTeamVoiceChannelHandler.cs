// <copyright file="CreateTeamVoiceChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.DTO;
using RSBingoBot.Requests;

internal class CreateTeamVoiceChannelHandler : RequestHandler<CreateTeamVoiceChannelRequest, Result>
{
    private const string ChannelName = "{0}-voice";

    private readonly DiscordChannelServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamVoiceChannelHandler(DiscordChannelServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamVoiceChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetCategory(request.TeamRole);

        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Voice, request.Category, overwrites);
        if (channel is null)
        {
            AddError(new CreateTeamVoiceChannelError());
        }
        else
        {
            request.Team.VoiceChannelId = channel.Id;
            AddSuccess(new CreateTeamVoiceChannelSuccess());
        }
    }
}