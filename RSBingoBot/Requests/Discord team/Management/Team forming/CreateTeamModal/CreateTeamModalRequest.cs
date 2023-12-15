// <copyright file="CreateTeamModalRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.EventArgs;

internal record CreateTeamModalRequest() : IModalRequest
{
    public ModalSubmitEventArgs InteractionArgs { get; set; }
}