﻿// <copyright file="ISelectComponentRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DiscordLibrary.DiscordComponents;

public interface ISelectComponentRequest : IComponentRequest<SelectComponent>, IInteractionRequest
{

}