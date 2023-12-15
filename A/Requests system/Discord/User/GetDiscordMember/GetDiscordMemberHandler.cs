// <copyright file="GetDiscordMemberHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;
using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetDiscordMemberHandler : RequestHandler<GetDiscordMemberRequest, DiscordMember?>
{
    protected override async Task<DiscordMember?> Process(GetDiscordMemberRequest request, CancellationToken cancellationToken)
    {
        // TODO: JR - test with a faulty id.
        DiscordMember member = await DataFactory.Guild.GetMemberAsync(request.Id);
        if (member is null)
        {
            AddError(new GetDiscordMemberError());
            return member;
        }

        AddSuccess(new GetDiscordMemberSuccess());
        return member;
    }
}