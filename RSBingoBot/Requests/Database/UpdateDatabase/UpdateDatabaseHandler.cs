// <copyright file="UpdateDatabaseHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

internal class UpdateDatabaseHandler : DatabaseRequestHandler<UpdateDatabaseRequest>
{
    protected override async Task Process(UpdateDatabaseRequest request, CancellationToken cancellationToken)
    {
        int count = request.DataWorker.SaveChanges();
        AddSuccess(new UpdateDatabaseSuccess(count));
    }
}