// <copyright file="IMessageCreatedRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public interface IMessageCreatedRequest : IRequest<Result>
{

}