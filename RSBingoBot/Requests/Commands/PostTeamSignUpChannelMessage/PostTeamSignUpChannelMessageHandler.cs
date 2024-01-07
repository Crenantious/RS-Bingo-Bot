// <copyright file="PostTeamSignUpChannelMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingoBot.DiscordComponents;

internal class PostTeamSignUpChannelMessageHandler : RequestHandler<PostTeamSignUpChannelMessageRequest>
{
    private readonly SingletonButtons singletonButtons;
    private readonly IDiscordMessageServices messageServices;

    public PostTeamSignUpChannelMessageHandler(SingletonButtons singletonButtons, IDiscordMessageServices messageServices)
    {
        this.singletonButtons = singletonButtons;
        this.messageServices = messageServices;
    }

    protected override async Task Process(PostTeamSignUpChannelMessageRequest request, CancellationToken cancellationToken)
    {
        Message message = new Message()
            .WithContent("Create a new team or join an existing one.")
            .AddComponents(singletonButtons.CreateTeam, singletonButtons.JoinTeam);
        await messageServices.Send(message, request.Channel);
        AddSuccess(new PostTeamSignUpChannelMessageSuccess());
    }
}