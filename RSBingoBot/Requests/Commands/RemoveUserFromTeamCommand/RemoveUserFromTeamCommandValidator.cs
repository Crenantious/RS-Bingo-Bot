// <copyright file="RemoveUserFromTeamCommandValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class RemoveUserFromTeamCommandValidator : BingoValidator<RemoveUserFromTeamCommandRequest>
{
    public RemoveUserFromTeamCommandValidator()
    {
        UserOnATeam(r => r.Member.Id);
    }
}