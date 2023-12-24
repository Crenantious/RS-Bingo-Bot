// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Validation;
using FluentResults;
using FluentValidation.Results;
using MediatR;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    private readonly Validator<TRequest> validator;

    public ValidationBehavior(Validator<TRequest> validator) =>
        this.validator = validator;

    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
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
            return Result.Fail<TResponse>(validationResult.Errors.Select(e => new ValidationError(e.ErrorMessage)));
        }

        var result = await next();

        foreach (SemaphoreSlim semaphore in validator.Semaphores)
        {
            semaphore.Release();
        }

        return result;
    }
}