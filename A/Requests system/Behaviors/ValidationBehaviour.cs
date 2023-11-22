// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    private readonly IValidator<TRequest> validator;

    public ValidationBehavior(IValidator<TRequest> validator) =>
        this.validator = validator;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
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