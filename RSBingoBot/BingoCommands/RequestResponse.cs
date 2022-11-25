// <copyright file="RequestResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

/// <summary>
/// Record to represent the request response.
/// </summary>
/// <param name="Success">If the request was successful.</param>
/// <param name="Response">The response.</param>
public record RequestResponse(bool Success, object Response)
{
    public bool IsSuccessful => Success;
    public bool Failed => !Success;
}

