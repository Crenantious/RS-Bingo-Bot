// <copyright file="CreateTeamGeneralChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingoBot.Requests;

internal class CreateTeamGeneralChannelHandler : RequestHandler<CreateTeamGeneralChannelRequest, Result>
{
    private const string ChannelName = "{0}-general";

    private readonly DiscordChannelServices channelServices;

    public CreateTeamGeneralChannelHandler(DiscordChannelServices channelServices)
    {
        this.channelServices = channelServices;
    }

    protected override async Task Process(CreateTeamGeneralChannelRequest request, CancellationToken cancellationToken)
    {
        string name = ChannelName.FormatConst(request.Team.Name);
        bool success = await channelServices.Create(name, DSharpPlus.ChannelType.Text);
        if (success)
        {
            AddSuccess(new CreateTeamGeneralChannelSuccess());
        }
        else
        {
            AddError(new CreateTeamGeneralChannelError());
        }
    }
}