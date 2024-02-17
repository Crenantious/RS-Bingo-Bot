// <copyright file="JoinTeamButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests.Validation;

internal class JoinTeamButtonValidator : BingoValidator<JoinTeamButtonRequest>
{
    public JoinTeamButtonValidator(SingletonButtons buttons)
    {
        ActiveInteractions<JoinTeamButtonRequest>(
            (r, c) => r.GetDiscordInteraction().User == c.Interaction.User,
            GetTooManyInteractionInstancesError(1, buttons.JoinTeam.Name), 1);

        ActiveInteractions<CreateTeamModalRequest>(
            (r, c) => r.GetDiscordInteraction().User == c.Interaction.User,
            CreateTeamButtonValidator.MultipleInstancesError, 1);

        UserNotOnATeam(r => r.GetDiscordInteraction().User, true);
    }
}