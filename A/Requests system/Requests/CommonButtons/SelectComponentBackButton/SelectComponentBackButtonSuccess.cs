// <copyright file="SelectComponentBackButtonSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using FluentResults;
using RSBingo_Common;

internal class SelectComponentBackButtonSuccess : Success
{
    private const string SuccessMessage = "Moved from page {0} to page {1}.";

    public SelectComponentBackButtonSuccess(SelectComponentPage movedFrom, SelectComponentPage movedTo) :
        base(SuccessMessage.FormatConst(movedFrom.Label, movedTo.Label))
    {

    }
}