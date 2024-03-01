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
            request.Message.AddImage(request.DiscordTeam.Board.Image);
            AddSuccess(new AddTeamBoardToMessageSuccess());
        }
        catch
        {
            AddError(new AddTeamBoardToMessageError());
        }
    }
}