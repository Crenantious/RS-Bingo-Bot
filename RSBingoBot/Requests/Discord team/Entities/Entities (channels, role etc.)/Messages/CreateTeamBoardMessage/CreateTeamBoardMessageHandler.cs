// <copyright file="CreateTeamBoardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using RSBingoBot.Discord;
using RSBingoBot.Imaging;
using RSBingoBot.Requests;

internal class CreateTeamBoardMessageHandler : RequestHandler<CreateTeamBoardMessageRequest, Message>
{
    private const string DropCodePrefix = "Drop code: {0}";
    private const string DropCodeNotSet = "not set";

    private readonly ButtonFactory buttonFactory;
    private readonly DiscordTeamBoardButtons buttons;

    public CreateTeamBoardMessageHandler(ButtonFactory buttonFactory, DiscordTeamBoardButtons buttons)
    {
        this.buttonFactory = buttonFactory;
        this.buttons = buttons;
    }

    protected override async Task<Message> Process(CreateTeamBoardMessageRequest request, CancellationToken cancellationToken)
    {
        Image boardImage = BoardImage.Create(request.Team);
        buttons.Create(request.DiscordTeam);

        string dropCode = string.IsNullOrWhiteSpace(request.Team.Code) ? DropCodeNotSet : request.Team.Code;

        var message = new Message()
            .WithContent(DropCodePrefix.FormatConst(dropCode))
            .AddComponents(buttons.changeTile, buttons.submitEvidence, buttons.submitDrop, buttons.viewEvidence)
            .AddImage(boardImage);

        // TODO: JR - implement
        //#if DEBUG
        //        Button clearEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Clear evidence"));
        //        Button completeNextTileEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Complete next tile"));
        //        message.AddComponents(clearEvidence, completeNextTileEvidence);
        //#endif

        AddSuccess(new CreateTeamBoardMessageSuccess());
        return message;
    }
}