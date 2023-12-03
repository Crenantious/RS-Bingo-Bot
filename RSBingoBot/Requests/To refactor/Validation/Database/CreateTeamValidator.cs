// <copyright file="CreateTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using RSBingoBot.Requests;

internal class CreateTeamValidator : Validator<CreateTeamButtonRequest>
{
    public CreateTeamValidator()
    {
        NewTeamName(r => r.TeamName);
    }
}