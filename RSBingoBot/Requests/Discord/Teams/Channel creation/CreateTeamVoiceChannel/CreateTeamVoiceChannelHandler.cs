// <copyright file="CreateTeamVoiceChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamVoiceChannelHandler : RequestHandler<CreateTeamVoiceChannelRequest>
{
    private const string ChannelName = "{0}-voice";

    private readonly IDiscordServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamVoiceChannelHandler(IDiscordServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamVoiceChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.DiscordTeam.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetVoice(request.DiscordTeam.Role!);

        Result<DiscordChannel> channel = await channelServices.CreateChannel(
            name, DSharpPlus.ChannelType.Voice, request.DiscordTeam.CategoryChannel, overwrites);

        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetVoiceChannel(channel.Value);
            AddSuccess(new CreateTeamVoiceChannelSuccess());
        }
        else
        {
            AddError(new CreateTeamVoiceChannelError());
        }
    }
}