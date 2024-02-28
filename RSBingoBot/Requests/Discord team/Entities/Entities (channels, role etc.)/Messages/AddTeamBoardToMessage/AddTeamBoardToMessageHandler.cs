﻿// <copyright file="AddTeamBoardToMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;
using RSBingoBot.Imaging;

internal class AddTeamBoardToMessageHandler : RequestHandler<AddTeamBoardToMessageRequest>
{
    protected override async Task Process(AddTeamBoardToMessageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            Image board = BoardImage.Create(request.Team);
            string boardPath = BoardImage.SaveBoard(board, request.Team.Name);
            request.Message.AddFile(boardPath);

            AddSuccess(new AddTeamBoardToMessageSuccess());
        }
        catch
        {
            AddError(new AddTeamBoardToMessageError());
        }
    }
}