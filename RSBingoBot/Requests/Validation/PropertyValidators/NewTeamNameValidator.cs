// <copyright file="NewTeamNameValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using FluentValidation.Validators;
using RSBingo_Common;
using RSBingo_Framework.Interfaces;

public class NewTeamNameValidator<T> : IPropertyValidator<T, string>
{
    private const string InvalidCharactersError = "A team name must only contain letters, numbers and/or spaces.";
    private const string TeamAlreadyExistsError = "A team with this name already exists.";
    private static readonly string NameTooLongError = $"A team name cannot exceed {General.TeamNameMaxLength} characters.";

    private readonly IDataWorker dataWorker;

    private bool isValid = true;

    public string Name => "NewTeamNameValidator";

    public NewTeamNameValidator(IDataWorker dataWorker) =>
        this.dataWorker = dataWorker;

    public string GetDefaultMessageTemplate(string errorCode) =>
        "Team name is invalid.";

    // TODO: JR - don't allow only white space and convert white space to a "-".
    private static bool ContainsSpecialCharacters(string name) =>
        name.Any(ch => (char.IsLetterOrDigit(ch) || ch is ' ') is false);

    public bool IsValid(ValidationContext<T> context, string name)
    {
        if (ContainsSpecialCharacters(name)) { AddFailure(context, InvalidCharactersError); }
        if (name.Length > General.TeamNameMaxLength) { AddFailure(context, NameTooLongError); }
        if (dataWorker.Teams.DoesTeamExist(name)) { AddFailure(context, TeamAlreadyExistsError); }

        return isValid;
    }

    private void AddFailure(ValidationContext<T> context, string failure)
    {
        isValid = false;
        context.AddFailure(failure);
    }
}