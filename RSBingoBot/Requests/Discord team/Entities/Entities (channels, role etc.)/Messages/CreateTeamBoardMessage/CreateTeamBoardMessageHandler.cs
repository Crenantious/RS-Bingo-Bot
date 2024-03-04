// <copyright file="CreateTeamBoardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

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
    private readonly DiscordTeamBoardButtons buttons;

    public CreateTeamBoardMessageHandler(ButtonFactory buttonFactory, DiscordTeamBoardButtons buttons)
    {
        this.buttonFactory = buttonFactory;
        this.buttons = buttons;
    }

    protected override async Task<Message> Process(CreateTeamBoardMessageRequest request, CancellationToken cancellationToken)
    {
        var teamServices = GetRequestService<IDiscordTeamServices>();

        buttons.Create(request.DiscordTeam);

        string dropCode = string.IsNullOrWhiteSpace(request.Team.Code) ? DropCodeNotSet : request.Team.Code;

        var message = new Message(request.DiscordTeam.BoardChannel!)
            .WithContent(DropCodePrefix.FormatConst(dropCode))
            .AddComponents(buttons.changeTile, buttons.submitEvidence);

        await teamServices.AddBoardToMessage(request.DiscordTeam, message);

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