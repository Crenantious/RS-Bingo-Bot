// <copyright file="ModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;

public abstract class ModalHandler<TRequest> : InteractionHandler<TRequest, ModalSubmitEventArgs>
    where TRequest : IModalRequest
{

}