// <copyright file="IRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using RSBingoBot.DTO;

internal interface IRequest
{
    public Task<RequestResult> Run();
}