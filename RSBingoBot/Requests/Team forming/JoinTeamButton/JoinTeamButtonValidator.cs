// <copyright file="JoinTeamButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class JoinTeamButtonValidator : BingoValidator<JoinTeamButtonRequest>
{
    public JoinTeamButtonValidator()
    {
        UserNotNull(r => r.User);
    }
}