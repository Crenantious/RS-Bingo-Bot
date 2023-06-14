// <copyright file="ICommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interfaces;

using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{

}