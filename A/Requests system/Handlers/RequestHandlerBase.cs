// <copyright file="RequestHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>, IRequestHandler
    where TRequest : IRequest<TResult>
    where TResult : ResultBase<TResult>, new()
{
    private const string UnexpectedError = "An unexpected error occurred.";

    private static int requestId = 0;

    private Dictionary<Type, string> exceptionMessages = new();
    private List<ISuccess> sucesses = new();
    private List<IError> errors = new();

    public int Id { get; private set; }

    public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken)
    {
        Id = requestId++;

        try
        {
            TResult result = await ProcessRequest(request, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            string error = exceptionMessages.ContainsKey(ex.GetType()) ?
                exceptionMessages[ex.GetType()] :
                UnexpectedError;
            return new TResult().WithError(error);
        }
    }

    /// <summary>
    /// <paramref name="message"/> will be added as an error if the exception type is thrown during <see cref="Process(TRequest, CancellationToken)"/>.
    /// <br/>If the type was already set, it will be overridden.
    /// </summary>
    protected void SetExceptionMessage<TException>(string message)
        where TException : Exception
    {
        exceptionMessages[typeof(TException)] = message;
    }

    private protected virtual async Task PreProcess(TRequest request, CancellationToken cancellationToken) { }
    private protected virtual async Task PostProcess(TRequest request, CancellationToken cancellationToken) { }

    private protected abstract Task<TResult> InternalProcess(TRequest request, CancellationToken cancellationToken);

    private async Task<TResult> ProcessRequest(TRequest request, CancellationToken cancellationToken)
    {
        await PreProcess(request, cancellationToken);
        TResult result = await InternalProcess(request, cancellationToken);
        await PostProcess(request, cancellationToken);

        return errors.Count == 0 ?
               result.WithSuccesses(sucesses) :
               result.WithErrors(errors);
    }

    #region Add result responses

    /// <summary>
    /// Add successes and errors to the <see cref="Result"/> response.
    /// </summary>
    protected void AddReasons(ResultBase result)
    {
        AddSuccesses(result.Successes);
        AddErrors(result.Errors);
    }

    /// <summary>
    /// Add a success to the <see cref="Result"/> response.
    /// </summary>
    protected void AddSuccess(ISuccess success) =>
        sucesses.Add(success);

    /// <summary>
    /// Add successes to the <see cref="Result"/> response.
    /// </summary>
    protected void AddSuccesses(IEnumerable<ISuccess> successes) =>
        sucesses.Concat(successes);

    /// <summary>
    /// Add a warning to the <see cref="Result"/> response.
    /// </summary>
    protected void AddWarning(IWarning warning) =>
        sucesses.Add(warning);

    /// <summary>
    /// Add warnings to the <see cref="Result"/> response.
    /// </summary>
    protected void AddWarnings(IEnumerable<IWarning> warnings) =>
        sucesses.Concat(warnings);

    /// <summary>
    /// Add an error to the <see cref="Result"/> response.
    /// </summary>
    protected void AddError(IError error) =>
        errors.Add(error);

    /// <summary>
    /// Add errors to the <see cref="Result"/> response.
    /// </summary>
    protected void AddErrors(IEnumerable<IError> errors) =>
        errors.Concat(errors);

    #endregion
}