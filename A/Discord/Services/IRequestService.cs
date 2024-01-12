// <copyright file="IRequestService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordServices;

using MediatR;

public interface IRequestService
{
    public void Initialise(IBaseRequest? parentRequest);
}