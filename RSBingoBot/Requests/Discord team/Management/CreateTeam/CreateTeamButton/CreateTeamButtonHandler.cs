// <copyright file="CreateTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;

internal class CreateTeamButtonHandler : ButtonHandler<CreateTeamButtonRequest>
{
    public const string ModalTeamNameKey = "Name";

    private readonly ModalFactory modalFactory;
    private readonly TextInputFactory textInputFactory;

    protected override bool SendKeepAliveMessage => false;

    public CreateTeamButtonHandler(ModalFactory modalFactory, TextInputFactory textInputFactory) : base()
    {
        this.modalFactory = modalFactory;
        this.textInputFactory = textInputFactory;
    }

    protected override async Task Process(CreateTeamButtonRequest request, CancellationToken cancellationToken)
    {
        var modalService = GetRequestService<IDiscordInteractionMessagingServices>();
        var modal = modalFactory.Create("Create team", request.GetDiscordInteraction())
            .AddComponents(textInputFactory.Create(new("Team name", ModalTeamNameKey)));

        await modalService.Send(modal, new CreateTeamModalRequest());
        AddSuccess(new CreateTeamButtonSuccess());
    }
}