// <copyright file="RenameRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class RenameRoleHandler : DiscordHandler<RenameRoleRequest>
{
    protected override async Task Process(RenameRoleRequest request, CancellationToken cancellationToken)
    {
        string oldName = request.Role.Name;
        await request.Role.ModifyAsync(name: request.NewName);
        AddSuccess(new RenameRoleSuccess(oldName, request.NewName));
    }
}