// <copyright file="CreateTeamHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

internal class CreateTeamHandler : RequestHandlerBase<CreateTeamRequest>
{
    private const string TeamSuccessfullyCreatedMessage = "The team '{0}' has been created successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public CreateTeamHandler() : base(semaphore)
    {

    }

    protected override async Task Process(CreateTeamRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}