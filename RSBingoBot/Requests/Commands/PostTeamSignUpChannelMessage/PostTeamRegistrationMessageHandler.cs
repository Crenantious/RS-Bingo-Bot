// <copyright file="PostTeamRegistrationMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using RSBingoBot.DiscordComponents;

internal class PostTeamRegistrationMessageHandler : CommandHandler<PostTeamRegistrationMessageRequest>
{
    private readonly SingletonButtons singletonButtons;
    private readonly MessageFactory messageFactory;

    public PostTeamRegistrationMessageHandler(SingletonButtons singletonButtons, MessageFactory messageFactory)
    {
        this.singletonButtons = singletonButtons;
        this.messageFactory = messageFactory;
    }

    protected override async Task Process(PostTeamRegistrationMessageRequest request, CancellationToken cancellationToken)
    {
        var messageServices = GetRequestService<IDiscordMessageServices>();
        Message message = messageFactory.Create(request.Channel)
            .WithContent("Create a new team or join an existing one.")
            .AddComponents(singletonButtons.CreateTeam, singletonButtons.JoinTeam);
        await messageServices.Send(message);
        AddSuccess(new PostTeamRegistrationMessageSuccess());
    }
}