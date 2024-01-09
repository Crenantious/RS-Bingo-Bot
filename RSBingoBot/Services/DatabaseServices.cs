// <copyright file="DatabaseServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

public class DatabaseServices : RequestService, IDatabaseServices
{
    public async Task<Result> Update(IDataWorker dataWorker) =>
        await RunRequest(new UpdateDatabaseRequest(dataWorker));
}