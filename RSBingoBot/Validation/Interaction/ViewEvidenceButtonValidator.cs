// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : InteractionValidator<ViewEvidenceButtonRequest>
{
    public ViewEvidenceButtonValidator()
    {
        TeamExists(r => r.Team);
        UserOnTeam(r => (r.Interaction.User, r.Team));
    }
}