// <copyright file="IInteraction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using MediatR;
using FluentResults;

public interface IInteraction : IRequest<Result>, IValidatable
{

}

public interface IInteraction<TResult> : IRequest<Result<TResult>>, IValidatable
{

}