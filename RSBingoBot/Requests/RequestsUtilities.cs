// <copyright file="RequestsUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DTO;
using RSBingoBot.Exceptions;
using RSBingo_Framework.Interfaces;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;

internal static class RequestsUtilities
{
    private const string InvalidCharactersError = "A team name must only contain letters, numbers and/or spaces.";
    private const string TeamAlreadyExistsError = "A team with this name already exists.";
    private const string UnableToDownloadFileResponse = "Unable to download the file. Please try again shortly.";

    private static readonly string NameTooLongError = $"A team name cannot exceed {General.TeamNameMaxLength} characters.";

    // TODO: JR - don't allow only white space and convert white space to a "-".
    public static RequestResult ValidateNewTeamName(string name, IDataWorker dataWorker)
    {
        List<string> errors = new(3);

        if (ContainsSpecialCharacters(name)) { errors.Add(InvalidCharactersError); }
        if (name.Length > General.TeamNameMaxLength) { errors.Add(NameTooLongError); }
        if (dataWorker.Teams.DoesTeamExist(name)) { errors.Add(TeamAlreadyExistsError); }

        if (errors.Any())
        {
            return new RequestResult(new TeamNameException(errors));
        }

        return new RequestResult(true);
    }

    private static bool ContainsSpecialCharacters(string name) =>
        name.Any(ch => (char.IsLetterOrDigit(ch) || ch is ' ') is false);

    public static DiscordRole GetTeamRole(IDataWorker dataWorker, string teamName) =>
        DataFactory.Guild.GetRole(dataWorker.Teams.GetByName(teamName)!.RoleId);

    public static DiscordRole GetRole(IDataWorker dataWorker, Team team) =>
        DataFactory.Guild.GetRole(team.RoleId);
}