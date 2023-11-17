﻿// <copyright file="EvidenceFoundSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using RSBingo_Framework.Models;

internal class EvidenceFoundSuccess : InteractionSuccess
{
    private const string SuccessMessage = "Evidence submitted for tile {0}:\n{1}.";

    public EvidenceFoundSuccess(Evidence evidence) :
        base(SuccessMessage.FormatConst(evidence.Tile.Task.Name, evidence.Url))
    {

    }
}