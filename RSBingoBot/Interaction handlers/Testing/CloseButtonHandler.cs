// <copyright file="CloseButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Interaction_handlers;

using FluentResults;
using RSBingoBot.Requests;

/// <summary>
/// Concludes the <see cref="CloseButtonRequest.InteractionHandler"/>.
/// </summary>
internal class CloseButtonHandler : InteractionHandler<CloseButtonRequest, Result>
{
    ///<inheritdoc/>
    public async override Task<Result> Handle(CloseButtonRequest request, CancellationToken cancellationToken)
    {
        await base.Handle(request, cancellationToken);

        request.InteractionHandler.Conclude();

        return Result.Ok();
    }
}