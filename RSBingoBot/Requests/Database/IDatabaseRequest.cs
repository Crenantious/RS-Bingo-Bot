// <copyright file="IDatabaseRequest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;
using MediatR;

public interface IDatabaseRequest : IRequest<Result>
{

}

public interface IDatabaseRequest<TResult> : IRequest<Result<TResult>>
{

}