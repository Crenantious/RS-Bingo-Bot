// <copyright file="IDatabaseServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;
using RSBingo_Framework.Interfaces;

public interface IDatabaseServices
{
    public Task<Result> Update(IDataWorker dataWorker);
}