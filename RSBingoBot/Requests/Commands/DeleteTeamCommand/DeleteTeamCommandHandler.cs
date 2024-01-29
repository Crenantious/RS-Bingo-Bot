// <copyright file="DeleteTeamCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;

internal class DeleteTeamCommandHandler : RequestHandler<DeleteTeamCommandRequest>
{
    protected override async Task Process(DeleteTeamCommandRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();
        await teamServices.Delete(request.Team);
    }
}