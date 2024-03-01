// <copyright file="SaveDatabaseChangesRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Interfaces;

internal record SaveDatabaseChangesRequest(IDataWorker DataWorker) : IDatabaseRequest;