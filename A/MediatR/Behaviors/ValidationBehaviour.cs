// <copyright file="ValidationBehavior.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Behaviours;

using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Validation;
using FluentResults;
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
        await WaitSemaphores(request);

        var errors = await Validate(request, cancellationToken);

        if (errors.Any())
        {
            ReleaseSemaphores(request);
            return new TResult().WithErrors(errors);
        }

        var result = await next();
        ReleaseSemaphores(request);

        return result;
    }

    private async Task WaitSemaphores(TRequest request)
    {
        foreach (SemaphoreSlim semaphore in validator.GetSemaphores(request))
        {
            await semaphore.WaitAsync();
        }
    }

    private void ReleaseSemaphores(TRequest request)
    {
        foreach (SemaphoreSlim semaphore in validator.GetSemaphores(request))
        {
            semaphore.Release();
        }
    }

    private async Task<IEnumerable<IError>> Validate(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await validator.ValidateAsync(request, cancellationToken);
            return result.Errors.Select(e => new ValidationError(e.ErrorMessage));
        }
        catch
        {
            return new IError[] { new ValidationInternalError() };
        }
    }
}