// <copyright file="CreateTeamModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Discord;
using RSBingoBot.Factories;

internal class CreateTeamModalHandler : ModalHandler<CreateTeamModalRequest>
{
    private readonly DiscordTeamFactory discordTeamFactory;

    public CreateTeamModalHandler(DiscordTeamFactory discordTeamFactory)
    {
        this.discordTeamFactory = discordTeamFactory;
    }

    protected override async Task Process(CreateTeamModalRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        var teamServices = GetRequestService<IDiscordTeamServices>();

        Result<DiscordTeam> discordTeam = await discordTeamFactory.CreateNew(
            request.GetInteractionArgs().Values[CreateTeamButtonHandler.ModalTeamNameKey],
            dataWorker);

        AddResponses(discordTeam);

        if (discordTeam.IsFailed)
        {
            return;
        }

        Result addToTeam = await teamServices.AddUserToTeam(request.GetDiscordInteraction().User, discordTeam.Value);
        AddResponses(addToTeam);
    }
}