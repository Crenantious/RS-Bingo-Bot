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
    private readonly MessageFactory messageFactory;

    public PostTeamSignUpChannelMessageHandler(SingletonButtons singletonButtons, IDiscordMessageServices messageServices,
        MessageFactory messageFactory)
    {
        this.singletonButtons = singletonButtons;
        this.messageServices = messageServices;
        this.messageFactory = messageFactory;
    }

    protected override async Task Process(PostTeamSignUpChannelMessageRequest request, CancellationToken cancellationToken)
    {
        Message message = messageFactory.Create(request.Channel)
            .WithContent("Create a new team or join an existing one.")
            .AddComponents(singletonButtons.CreateTeam, singletonButtons.JoinTeam);
        await messageServices.Send(message);
        AddSuccess(new PostTeamSignUpChannelMessageSuccess());
    }
}