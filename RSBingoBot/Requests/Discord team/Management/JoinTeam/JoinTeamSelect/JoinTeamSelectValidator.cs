// <copyright file="JoinTeamSelectValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Requests.Validation;

internal class JoinTeamSelectValidator : BingoValidator<JoinTeamSelectRequest>
{
    public JoinTeamSelectValidator()
    {
        UserInteraction(r => r.GetDiscordInteraction().User);
        UserNotNull(r => r.GetDiscordInteraction().User);
    }

    protected override IEnumerable<SemaphoreSlim> GetSemaphores(JoinTeamSelectRequest request, RequestSemaphores semaphores) =>
        new List<SemaphoreSlim>()
        {
            semaphores.GetTeamRegistration(request.GetDiscordInteraction().User.Id)
        };
}