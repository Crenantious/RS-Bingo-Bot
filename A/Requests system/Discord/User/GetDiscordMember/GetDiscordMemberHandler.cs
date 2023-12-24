// <copyright file="GetDiscordMemberHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;

internal class GetDiscordMemberHandler : DiscordHandler<GetDiscordMemberRequest, DiscordMember?>
{
    protected override async Task<DiscordMember?> Process(GetDiscordMemberRequest request, CancellationToken cancellationToken)
    {
        // TODO: JR - test with a faulty id.
        DiscordMember member = await DataFactory.Guild.GetMemberAsync(request.Id);
        AddSuccess(new GetDiscordMemberSuccess(member));
        return member;
    }
}