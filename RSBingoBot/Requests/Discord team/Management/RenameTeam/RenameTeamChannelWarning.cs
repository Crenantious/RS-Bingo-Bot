// <copyright file="RenameTeamChannelWarning.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests;

internal class RenameTeamChannelWarning : IWarning, IDiscordResponse
{
    private const string ErrorMessage = "Failed to rename the team's {0} channel.";

    public string Message { get; }
    public Dictionary<string, object> Metadata { get; } = new();

    /// <param name="channelTypeName">e.g. category, board, voice.</param>
    public RenameTeamChannelWarning(string channelTypeName)
    {
        Message = ErrorMessage.FormatConst(channelTypeName);
    }
}