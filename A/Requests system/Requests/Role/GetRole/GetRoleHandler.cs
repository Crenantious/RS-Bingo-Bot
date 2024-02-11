// <copyright file="GetRoleHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetRoleHandler : DiscordHandler<GetRoleRequest, DiscordRole>
{
    protected override async Task<DiscordRole> Process(GetRoleRequest request, CancellationToken cancellationToken)
    {
        DiscordRole role = DataFactory.Guild.GetRole(request.Id);
        if (role is null)
        {
            AddError(new GetRoleError(request.Id));
        }
        else
        {
            AddSuccess(new GetRoleSuccess(role));
        }

        // Don't care if it's null since we've added an error so the consumer should check that first.
        return role!;
    }
}