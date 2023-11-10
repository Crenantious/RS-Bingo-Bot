// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Behaviours;

using MediatR;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using RSBingoBot.Errors;
using RSBingoBot.Interfaces;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result>
    where TRequest : IValidatable<TResult>
{
    private readonly IValidator<TRequest> validator;

    public ValidationBehavior(IValidator<TRequest> validator) =>
        this.validator = validator;

    public async Task<Result> Handle(TRequest request, RequestHandlerDelegate<Result> next, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            Result response = new();

            foreach (ValidationFailure error in validationResult.Errors)
            {
                response.WithError(new ValidationError(error.ErrorMessage));
            }

            return response;
        }

        return await next();
    }
}