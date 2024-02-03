// <copyright file="SendRequestResultResponsesSuccess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using FluentResults;

internal class SendRequestResultResponsesSuccess : Success
{
    private const string SuccessMessage = "";

    public SendRequestResultResponsesSuccess() : base(SuccessMessage)
    {

    }
}