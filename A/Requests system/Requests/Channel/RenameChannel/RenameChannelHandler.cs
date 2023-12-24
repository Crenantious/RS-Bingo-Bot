// <copyright file="RenameChannelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Requests;

using DSharpPlus.Net.Models;

internal class RenameChannelHandler : DiscordHandler<RenameChannelRequest>
{
    protected override async Task Process(RenameChannelRequest request, CancellationToken cancellationToken)
    {
        string oldName = request.Channel.Name;
        await request.Channel.ModifyAsync((model) => Rename(request.NewName, model));
        AddSuccess(new RenameChannelSuccess(oldName, request.NewName));
    }

    private void Rename(string newName, ChannelEditModel model)
    {
        model.Name = newName;
    }
}