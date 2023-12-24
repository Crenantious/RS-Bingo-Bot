// <copyright file="IDiscordRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public interface IDiscordRequest : IRequest<Result>
{

}

public interface IDiscordRequest<TResult> : IRequest<Result<TResult>>
{

}