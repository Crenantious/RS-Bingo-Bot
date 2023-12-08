// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Validation;
using FluentResults;
using FluentValidation.Results;
using MediatR;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult>>
    where TRequest : IRequest<Result<TResult>>
{
    private readonly Validator<TRequest> validator;

    public ValidationBehavior(Validator<TRequest> validator) =>
        this.validator = validator;

    public async Task<Result<TResult>> Handle(TRequest request, RequestHandlerDelegate<Result<TResult>> next, CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        foreach (SemaphoreSlim semaphore in validator.Semaphores)
        {
            await semaphore.WaitAsync();
        }

        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result.Fail<TResult>(validationResult.Errors.Select(e => new ValidationError(e.ErrorMessage)));
        }

        var result = await next();

        foreach (SemaphoreSlim semaphore in validator.Semaphores)
        {
            semaphore.Release();
        }

        return result;
    }
}