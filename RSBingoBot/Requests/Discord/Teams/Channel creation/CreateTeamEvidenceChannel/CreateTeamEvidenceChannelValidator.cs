// <copyright file="CreateTeamEvidenceChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamEvidenceChannelValidator : Validator<CreateTeamEvidenceChannelRequest>
{
    public CreateTeamEvidenceChannelValidator()
    {
        TeamExists(r => r.Team);
        ChannelNotNull(r => r.Category);
    }
}