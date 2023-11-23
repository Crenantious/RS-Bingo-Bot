// <copyright file="IDiscordInteractionMessagingServices.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using DiscordLibrary.DiscordEntities;

public interface IDiscordInteractionMessagingServices
{
    public Task<bool> Send(InteractionMessage message);
    public Task<bool> Update(InteractionMessage message);
    public Task<bool> Delete(InteractionMessage message);
}