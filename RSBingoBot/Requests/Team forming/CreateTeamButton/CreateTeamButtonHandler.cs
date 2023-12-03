// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.RequestHandlers;

internal class CreateTeamButtonHandler : ButtonHandler<CreateTeamButtonRequest>
{
    private readonly TextInputFactory textInputFactory;

    public CreateTeamButtonHandler(TextInputFactory textInputFactory) : base()
    {
        this.textInputFactory = textInputFactory;
    }

    protected override async Task Process(CreateTeamButtonRequest request, CancellationToken cancellationToken)
    {
        var modal = new Modal("Create team", request.InteractionArgs.Interaction)
            // TODO: JR - check if the text input id will suffice.
            .AddComponents(textInputFactory.Create(new("Team name", "Team name")));

        ResponseMessages.Add(modal);
        AddSuccess(new CreateTeamButtonSuccess());
    }
}