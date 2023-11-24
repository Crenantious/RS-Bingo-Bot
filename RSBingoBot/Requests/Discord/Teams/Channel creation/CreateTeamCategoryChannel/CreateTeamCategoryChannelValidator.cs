// <copyright file="CreateTeamCategoryChannelValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using DiscordLibrary.Requests.Validation;
using RSBingoBot.Requests;

internal class CreateTeamCategoryChannelValidator : Validator<CreateTeamCategoryChannelRequest>
{
    public CreateTeamCategoryChannelValidator()
    {
        TeamExists(r => r.Team);
    }
}