// <copyright file="ModalHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

public abstract class ModalHandler<TRequest> : InteractionHandler<TRequest>
    where TRequest : IModalRequest
{

}