// <copyright file="ViewEvidenceButtonValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingoBot.Requests;

internal class ViewEvidenceButtonValidator : AbstractValidator<ViewEvidenceButtonRequest>
{
    public ViewEvidenceButtonValidator()
    {
        this.UserNotNull();
    }
}