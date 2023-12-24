// <copyright file="ISelectComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;

// TODO: JR - decide how to handler this possible inheriting IInteractable.
public interface ISelectComponentRequest : IComponentInteractionRequest<SelectComponent>
{

}