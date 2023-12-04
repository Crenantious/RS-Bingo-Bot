﻿// <copyright file="CreateTeamModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.RequestHandlers;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;
using RSBingoBot.Factories;

internal class CreateTeamModalHandler : ModalHandler<CreateTeamModalRequest>
{
    private readonly DiscordTeamFactory discordTeamFactory;
    private readonly IDiscordTeamServices teamServices;

    public CreateTeamModalHandler(DiscordTeamFactory discordTeamFactory, IDiscordTeamServices teamServices)
    {
        this.discordTeamFactory = discordTeamFactory;
        this.teamServices = teamServices;
    }

    protected override async Task Process(CreateTeamModalRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        Result<DiscordTeam> discordTeam = await discordTeamFactory.CreateNew(
            InteractionArgs.Values[CreateTeamButtonHandler.ModalTeamNameKey],
            dataWorker);

        if (discordTeam.IsFailed)
        {
            AddError(new CreateTeamModalError());
            return;
        }
        AddSuccess(new CreateTeamModalSuccess(discordTeam.Value.Team));

        Result addToTeam = await teamServices.AddUserToTeam(request.InteractionArgs.Interaction.User, discordTeam.Value);

        if (addToTeam.IsFailed)
        {
            AddError(new CreateTeamModalError());
        }
    }
}