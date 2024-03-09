// <copyright file="UpdateLeaderboardValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Validation;
using FluentValidation;

internal class UpdateLeaderboardValidator : Validator<UpdateLeaderboardRequest>
{
    private const string NullLeaderboardMessage = "The leaderboard message is null. Make sure the id is set in the config correctly.";
    private const string InvalidNumberOfFiles = "The given message must contain only 1 file. " +
        "Make sure the message that is set is the one generated from {0}.";

    public UpdateLeaderboardValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        NotNull(r => r.Message, NullLeaderboardMessage);
        RuleFor(r => r.Message!.Files.Count())
            .Equal(1)
            .WithMessage(InvalidNumberOfFiles.FormatConst(typeof(PostLeaderboardCommandRequest)));
    }
}