// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Validation;
using FluentResults;
using FluentValidation.Results;
using MediatR;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private readonly Validator<TRequest> validator;

    public ValidationBehavior(Validator<TRequest> validator) =>
        this.validator = validator;

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        foreach (SemaphoreSlim semaphore in validator.Semaphores)
        {
            await semaphore.WaitAsync();
        }

        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            ReleaseSemaphores();
            return new TResult().WithErrors(validationResult.Errors.Select(e => new ValidationError(e.ErrorMessage)));
        }

        var result = await next();
        ReleaseSemaphores();

        return result;
    }

    private void ReleaseSemaphores()
    {
        foreach (SemaphoreSlim semaphore in validator.Semaphores)
        {
            semaphore.Release();
        }
    }
}