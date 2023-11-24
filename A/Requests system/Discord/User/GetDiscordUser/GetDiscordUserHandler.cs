// <copyright file="GetDiscordUserHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetDiscordUserHandler : RequestHandler<GetDiscordUserRequest, DiscordUser>
{
    protected override async Task<DiscordUser> Process(GetDiscordUserRequest request, CancellationToken cancellationToken)
    {
        DiscordUser user = await DataFactory.Guild.GetMemberAsync(request.Id);
        if (user is null)
        {
            AddError(new GetDiscordUserError());
            return user!;
        }

        AddSuccess(new GetDiscordUserSuccess());
        return user;
    }
}