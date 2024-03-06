// <copyright file="ChangeTilesTaskSelectBackButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;

internal class ChangeTilesTaskSelectBackButtonHandler : SelectComponentBackButtonHandler<ChangeTilesTaskSelectBackButtonRequest>
{
    protected override void Process(ChangeTilesTaskSelectBackButtonRequest request,
        SelectComponentPage previousPage, SelectComponentPage currentPage)
    {
        request.DTO.ChangeToTask = null;
    }
}