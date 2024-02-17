// <copyright file="CreateTeamButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.DiscordComponents;
using RSBingoBot.Requests.Validation;

internal class CreateTeamButtonValidator : BingoValidator<CreateTeamButtonRequest>
{
    public const string MultipleInstancesError = "You cannot create and join a team at the same time.";

    public CreateTeamButtonValidator(SingletonButtons buttons)
    {
        ActiveInteractions<CreateTeamModalRequest>(
            (r, c) => r.GetDiscordInteraction().User == c.Interaction.User,
            GetTooManyInteractionInstancesError(1, buttons.CreateTeam.Name), 1);

        ActiveInteractions<JoinTeamButtonRequest>(
            (r, c) => r.GetDiscordInteraction().User == c.Interaction.User,
            MultipleInstancesError, 1);

        UserNotOnATeam(r => r.GetDiscordInteraction().User, true);
    }
}