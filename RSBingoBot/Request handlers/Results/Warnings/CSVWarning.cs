// <copyright file="CSVWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

internal record CSVWarning(string Message) : Warning(Message)
{

}