// <copyright file="IModalRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.EventArgs;

public interface IModalRequest : IInteractionRequest<ModalSubmitEventArgs>
{

}