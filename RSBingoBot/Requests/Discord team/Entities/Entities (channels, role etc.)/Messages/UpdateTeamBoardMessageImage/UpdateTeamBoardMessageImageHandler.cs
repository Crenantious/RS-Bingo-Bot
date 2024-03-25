// <copyright file="UpdateTeamBoardMessageImageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using Imaging.Board;

internal class UpdateTeamBoardMessageImageHandler : RequestHandler<UpdateTeamBoardMessageImageRequest>
{
    protected override async Task Process(UpdateTeamBoardMessageImageRequest request, CancellationToken cancellationToken)
    {
        Board board = request.DiscordTeam.Board;
        Message boardMessage = request.DiscordTeam.BoardMessage!;
        MessageFile boardMessageFile = boardMessage.Files.ElementAt(0);
        var messageServices = GetRequestService<IDiscordMessageServices>();

        board.UpdateTiles(request.Team, request.BoardIndexes);
        boardMessageFile.SetContent(board.Image, board.FileExtension);
        await messageServices.Update(boardMessage);
    }
}