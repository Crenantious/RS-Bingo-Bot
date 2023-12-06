// <copyright file="SubmitDropMessageValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentValidation;
using RSBingoBot.Requests.Validation;

internal class SubmitDropMessageValidator : BingoValidator<SubmitDropMessageRequest>
{
    // TODO: JR - use ImageSharp for image validation.
    private readonly string[] mediaTypes = new string[] { "png", "bmp", "jpg" };
    private string ErrorMessage = "Attachment must be of type: png, bmp or jpg";

    public SubmitDropMessageValidator()
    {
        RuleFor(r => r.MessageArgs.Message.Attachments.ElementAt(0).MediaType)
            .Must(t => mediaTypes.Contains(t))
            .WithMessage(ErrorMessage);
    }
}