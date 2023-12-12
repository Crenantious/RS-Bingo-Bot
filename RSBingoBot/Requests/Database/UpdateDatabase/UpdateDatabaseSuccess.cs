// <copyright file="UpdateDatabaseSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;

internal class UpdateDatabaseSuccess : Success
{
    private const string SuccessMessage = "{0} items have been updated successfully in the database.";

    public UpdateDatabaseSuccess(int updateCount) : base(SuccessMessage.FormatConst(updateCount))
    {

    }
}