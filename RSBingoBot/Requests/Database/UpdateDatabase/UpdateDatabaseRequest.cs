// <copyright file="UpdateDatabaseRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Interfaces;

internal record UpdateDatabaseRequest(IDataWorker DataWorker) : IDatabaseRequest;