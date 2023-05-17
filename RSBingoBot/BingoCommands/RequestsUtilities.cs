// <copyright file="RequestsUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.Interfaces;

internal static class RequestsUtilities
{
    private const string InvalidCharactersError = "A team name must only contain letters and/or numbers.";
    private const string TeamAlreadyExistsError = "A team with this name already exists.";

    private static readonly string NameTooLongError = $"A team name cannot exceed {General.TeamNameMaxLength} characters.";

    public static IEnumerable<string> GetNewTeamNameErrors(string name, IDataWorker dataWorker)
    {
        List<string> errors = new(2);

        if (ContainsSpecialCharacters(name)) { errors.Add(InvalidCharactersError); }
        if (name.Length > General.TeamNameMaxLength) { errors.Add(NameTooLongError); }
        if (dataWorker.Teams.DoesTeamExist(name)) { errors.Add(TeamAlreadyExistsError); }

        return errors;
    }

    private static bool ContainsSpecialCharacters(string name) =>
        name.Any(ch => char.IsLetterOrDigit(ch) is false);
}