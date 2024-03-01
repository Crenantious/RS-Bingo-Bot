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

internal class CreateTeamModalHandler : ModalHandler<CreateTeamModalRequest>
{
    protected override async Task Process(CreateTeamModalRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        var teamServices = GetRequestService<IDiscordTeamServices>();
        var dbServices = GetRequestService<IDatabaseServices>();

        Result<DiscordTeam> discordTeam = await teamServices.CreateNewTeam(GetTeamName(request), dataWorker);

        if (discordTeam.IsSuccess)
        {
            await teamServices.AddUserToTeam(request.GetDiscordInteraction().User, discordTeam.Value, dataWorker);
            await dbServices.SaveChanges(dataWorker);
        }

        await InteractionTracker.ConcludeInteraction();
    }

    private static string GetTeamName(CreateTeamModalRequest request) =>
        request.GetInteractionArgs().Values[CreateTeamButtonHandler.ModalTeamNameKey];
}