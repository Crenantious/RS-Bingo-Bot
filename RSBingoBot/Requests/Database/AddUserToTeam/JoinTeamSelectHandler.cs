// <copyright file="JoinTeamSelectHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;

internal class JoinTeamSelectHandler : SelectComponentHandler<JoinTeamSelectRequest>
{
    private readonly IDiscordServices discordServices;
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public JoinTeamSelectHandler(IDiscordServices discordServices)
    {
        this.discordServices = discordServices;
    }

    protected override async Task OnItemSelectedAsync(IEnumerable<SelectComponentItem> items, JoinTeamSelectRequest request, CancellationToken cancellationToken)
    {
        DiscordTeam discordTeam = (DiscordTeam)items.ElementAt(0).Value!;

        dataWorker.Users.Create(request.User.Id, discordTeam.Team);
        dataWorker.SaveChanges();
        AddSuccess(new JoinTeamSelectAddedToTeamSuccess(request.User), false);

        Result<DSharpPlus.Entities.DiscordMember> member = await discordServices.GetMember(request.User.Id);

        if (member.IsFailed)
        {
            AddErrors(member.Errors);
            return;
        }

        Result result = await discordServices.GrantRole(member.Value, discordTeam.Role!);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
            return;
        }

        AddSuccess(new JoinTeamSelectSuccess(discordTeam.Team));
    }
}