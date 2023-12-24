// <copyright file="RevokeRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class RevokeRoleHandler : DiscordHandler<RevokeRoleRequest>
{
    protected override async Task Process(RevokeRoleRequest request, CancellationToken cancellationToken)
    {
        await request.Member.RevokeRoleAsync(request.Role);
        AddSuccess(new RevokeRoleSuccess(request.Member, request.Role));
    }
}