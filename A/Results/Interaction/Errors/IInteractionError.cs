﻿// <copyright file="IInteractionError.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

public interface IInteractionError : IError, IInteractionReason
{

}