// <copyright file="RequestResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DTO;

internal class RequestResult
{
    public bool IsSuccessful { get; }
    public bool IsFaulted { get; }
    public IEnumerable<string> Responses { get; }

    private RequestResult(IEnumerable<string> responses, bool isSuccessful)
    {
        Responses = responses;
        IsSuccessful = isSuccessful;
        IsFaulted = !isSuccessful;
    }

    public static RequestResult Success(IEnumerable<string> responses) =>
        new(responses, false);

    public static RequestResult Success(string response) =>
        new(new string[] { response }, false);

    public static RequestResult Failed(IEnumerable<string> responses) =>
        new(responses, false);

    public static RequestResult Failed(string response) =>
        new(new string[] { response }, false);
}