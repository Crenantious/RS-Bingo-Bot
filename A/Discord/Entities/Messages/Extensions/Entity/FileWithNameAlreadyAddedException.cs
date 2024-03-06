// <copyright file="MessageExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordEntities;

using RSBingo_Common;

internal class FileWithNameAlreadyAddedException : Exception
{
    private const string Message = "A file with the name {0} has already been added to this message.";

    public FileWithNameAlreadyAddedException(string fileName) : base(Message.FormatConst(fileName))
    {

    }
}