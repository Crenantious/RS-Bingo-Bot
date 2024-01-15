// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;

internal class CreateTeamButtonHandler : ButtonHandler<CreateTeamButtonRequest>
{
    public const string ModalTeamNameKey = "Name";

    private readonly TextInputFactory textInputFactory;

    public CreateTeamButtonHandler(TextInputFactory textInputFactory) : base()
    {
        this.textInputFactory = textInputFactory;
    }

    protected override async Task Process(CreateTeamButtonRequest request, CancellationToken cancellationToken)
    {
        var modalService = GetRequestService<IDiscordInteractionMessagingServices>();
        var modal = new Modal("Create team", request.InteractionArgs.Interaction)
            .AddComponents(textInputFactory.Create(new("Team name", ModalTeamNameKey)));

        await modalService.Send(modal, new CreateTeamModalRequest());
        AddSuccess(new CreateTeamButtonSuccess());
    }
}