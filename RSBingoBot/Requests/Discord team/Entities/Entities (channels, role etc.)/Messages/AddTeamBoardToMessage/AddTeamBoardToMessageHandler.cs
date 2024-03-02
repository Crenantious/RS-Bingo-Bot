// <copyright file="AddTeamBoardToMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Requests;

internal class AddTeamBoardToMessageHandler : RequestHandler<AddTeamBoardToMessageRequest>
{
    protected override async Task Process(AddTeamBoardToMessageRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var board = request.DiscordTeam.Board;

            MessageFile file = new("Board");
            file.SetContent(board.Image, board.FileExtension);

            request.Message.AddFile(file);

            AddSuccess(new AddTeamBoardToMessageSuccess());
        }
        catch
        {
            AddError(new AddTeamBoardToMessageError());
        }
    }
}