// <copyright file="CreateTeamModalValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class CreateTeamModalValidator : BingoValidator<CreateTeamModalRequest>
{
    public CreateTeamModalValidator()
    {
        UserNotOnATeam(r => r.GetDiscordInteraction().User, true);
        NewTeamName(r => r.GetInteractionArgs().Values[CreateTeamButtonHandler.ModalTeamNameKey]);
    }
}