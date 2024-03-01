// <copyright file="SaveDatabaseChangesHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

internal class SaveDatabaseChangesHandler : DatabaseRequestHandler<SaveDatabaseChangesRequest>
{
    protected override async Task Process(SaveDatabaseChangesRequest request, CancellationToken cancellationToken)
    {
        int count = request.DataWorker.SaveChanges();
        AddSuccess(new SaveDatabaseChangesSuccess(count));
    }
}