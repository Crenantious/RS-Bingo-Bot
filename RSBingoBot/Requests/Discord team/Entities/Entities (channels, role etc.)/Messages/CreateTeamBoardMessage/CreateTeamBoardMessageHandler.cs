// <copyright file="CreateTeamBoardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using RSBingoBot.Discord;
using RSBingoBot.Requests;

internal class CreateTeamBoardMessageHandler : RequestHandler<CreateTeamBoardMessageRequest, Message>
{
    private const string DropCodePrefix = "Drop code: {0}";
    private const string DropCodeNotSet = "not set";

    private readonly ButtonFactory buttonFactory;
    private readonly MessageFactory messageFactory;
    private readonly DiscordTeamBoardButtons buttons;

    public CreateTeamBoardMessageHandler(ButtonFactory buttonFactory, MessageFactory messageFactory, DiscordTeamBoardButtons buttons)
    {
        this.buttonFactory = buttonFactory;
        this.messageFactory = messageFactory;
        this.buttons = buttons;
    }

    protected override async Task<Message> Process(CreateTeamBoardMessageRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();

        buttons.Create(request.DiscordTeam);

        string dropCode = string.IsNullOrWhiteSpace(request.Team.Code) ? DropCodeNotSet : request.Team.Code;
        Button submissionButton = General.HasCompetitionStarted ? buttons.SubmitDrop : buttons.SubmitEvidence;

        Message message = messageFactory.Create(request.DiscordTeam.BoardChannel!)
            .WithContent(DropCodePrefix.FormatConst(dropCode))
            .AddComponents(buttons.ChangeTile, submissionButton);

        await teamServices.AddBoardToMessage(request.DiscordTeam, message);

        AddSuccess(new CreateTeamBoardMessageSuccess());
        return message;
    }
}