// <copyright file="CreateTeamCategoryChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamCategoryChannelHandler : RequestHandler<CreateTeamCategoryChannelRequest>
{
    private const string ChannelName = "{0}";

    private readonly IDiscordServices channelServices;
    private readonly DiscordTeamChannelOverwrites channelOverwrites;

    public CreateTeamCategoryChannelHandler(IDiscordServices channelServices, DiscordTeamChannelOverwrites channelOverwrites)
    {
        this.channelServices = channelServices;
        this.channelOverwrites = channelOverwrites;
    }

    protected override async Task Process(CreateTeamCategoryChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.DiscordTeam.Team.Name);
        DiscordOverwriteBuilder[] overwrites = channelOverwrites.GetCategory(request.DiscordTeam.Role!);

        Result<DiscordChannel> channel = await channelServices.CreateChannel(name, DSharpPlus.ChannelType.Category, overwrites: overwrites);
        if (channel.IsSuccess)
        {
            request.DiscordTeam.SetCategoryChannel(channel.Value);
            AddSuccess(new CreateTeamCategoryChannelSuccess());
        }
        else
        {
            AddError(new CreateTeamCategoryChannelError());
        }
    }
}