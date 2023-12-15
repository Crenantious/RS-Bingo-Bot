// <copyright file="AddUserToTeamValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class AddUserToTeamValidator : BingoValidator<AddUserToTeamRequest>
{
    public AddUserToTeamValidator()
    {
        UserNotNull(r => r.User);
        UserNotOnATeam(r => r.User);
    }
}