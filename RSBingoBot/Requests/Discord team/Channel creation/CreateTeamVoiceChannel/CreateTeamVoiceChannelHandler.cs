// <copyright file="CreateTeamVoiceChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamVoiceChannelHandler : RequestHandler<CreateTeamVoiceChannelRequest, Result>
{
    private const string ChannelName = "{0}-voice";

    private readonly DiscordChannelServices channelServices;

    public CreateTeamVoiceChannelHandler(DiscordChannelServices channelServices)
    {
        this.channelServices = channelServices;
    }

    protected override async Task Process(CreateTeamVoiceChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        DiscordChannel? channel = await channelServices.Create(name, DSharpPlus.ChannelType.Category);
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