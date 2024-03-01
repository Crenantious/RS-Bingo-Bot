// <copyright file="AddTeamBoardToMessageHandler.cs" company="PlaceholderCompany">
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
            var path = BoardImage.SaveBoard(board, request.Team.Name);

            MessageFile file = new("Board");
            file.SetContent(path);

            request.Message.AddFile(file);

            AddSuccess(new AddTeamBoardToMessageSuccess());
        }
        catch
        {
            AddError(new AddTeamBoardToMessageError());
        }
    }
}