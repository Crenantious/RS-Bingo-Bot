// <copyright file="DeleteRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

internal class DeleteRoleHandler : DiscordHandler<DeleteRoleRequest>
{
    protected override async Task Process(DeleteRoleRequest request, CancellationToken cancellationToken)
    {
        await request.Role.DeleteAsync();
        AddSuccess(new DeleteRoleSuccess(request.Role));
    }
}