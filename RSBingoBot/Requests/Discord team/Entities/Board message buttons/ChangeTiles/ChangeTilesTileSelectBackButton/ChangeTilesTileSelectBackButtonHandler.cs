// <copyright file="ChangeTilesTileSelectBackButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;

internal class ChangeTilesTileSelectBackButtonHandler : SelectComponentBackButtonHandler<ChangeTilesTileSelectBackButtonRequest>
{
    protected override void Process(ChangeTilesTileSelectBackButtonRequest request,
        SelectComponentPage previousPage, SelectComponentPage currentPage)
    {
        request.DTO.ChangeFromTileBoardIndex = null;
    }
}