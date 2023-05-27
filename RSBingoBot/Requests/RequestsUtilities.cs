// <copyright file="RequestsUtilities.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingoBot.DTO;
using RSBingoBot.Exceptions;
using RSBingo_Framework.Interfaces;
using System.Net;
using RSBingo_Framework.Exceptions;
using static RSBingoBot.MessageUtilities;
using RSBingo_Framework;

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

    public static Result DownloadFile(Uri uri, string fileName)
    {
        WebClient webClient = new();

        try
        {
            if (WhitelistChecker.IsUriWhitelisted(uri) is false)
            {
                throw new UnpermittedURLException(WhitelistChecker.GetWhitelistedDomains());
            }
            webClient.DownloadFile(uri, fileName);
        }
        catch (WebException e)
        {
            return new(new WebException(UnableToDownloadFileResponse));
        }
        catch (Exception e)
        {
            return new(e);
        }

        return new();
    }

    private static bool ContainsSpecialCharacters(string name) =>
        name.Any(ch => (char.IsLetterOrDigit(ch) || ch is ' ') is false);
}