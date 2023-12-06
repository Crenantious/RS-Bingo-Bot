﻿// <copyright file="JoinTeamSelectValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.Requests.Validation;

internal class JoinTeamSelectValidator : BingoValidator<JoinTeamSelectRequest>
{
    public JoinTeamSelectValidator()
    {
        UserNotNull(r => r.User);
    }
}