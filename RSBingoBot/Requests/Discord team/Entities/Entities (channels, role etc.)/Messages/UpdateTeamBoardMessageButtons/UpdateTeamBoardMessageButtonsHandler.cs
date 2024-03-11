// <copyright file="UpdateTeamBoardMessageButtonsHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingoBot.Discord;

internal class UpdateTeamBoardMessageButtonsHandler : RequestHandler<UpdateTeamBoardMessageButtonsRequest>
{
    private readonly DiscordTeamBoardButtons buttons;

    public UpdateTeamBoardMessageButtonsHandler(DiscordTeamBoardButtons buttons)
    {
        this.buttons = buttons;
    }

    protected override async Task Process(UpdateTeamBoardMessageButtonsRequest request, CancellationToken cancellationToken)
    {
        List<IComponent> components = GetComponentRow(request);

        Button submissionButton = General.HasCompetitionStarted ? buttons.SubmitDrop : buttons.SubmitEvidence;
        components[1] = submissionButton;

        var messageServices = GetRequestService<IDiscordMessageServices>();
        await messageServices.Update(request.Message);
    }

    private List<IComponent> GetComponentRow(UpdateTeamBoardMessageButtonsRequest request) =>
        request.Message.Components
            .GetRows()
            .ElementAt(0);
}