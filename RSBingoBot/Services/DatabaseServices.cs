// <copyright file="DatabaseServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

public class DatabaseServices : IDatabaseServices
{
    public async Task<Result> Update(IDataWorker dataWorker) =>
        await RequestRunner.Run(new UpdateDatabaseRequest(dataWorker));
}