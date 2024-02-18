// <copyright file="CreateTeamBoardMessageHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.RequestHandlers;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using RSBingoBot.Imaging;
using RSBingoBot.Requests;

internal class CreateTeamBoardMessageHandler : RequestHandler<CreateTeamBoardMessageRequest, Message>
{
    private const string DropCodePrefix = "Drop code: {0}";
    private const string DropCodeNotSet = "not set";

    private readonly ButtonFactory buttonFactory;

    public CreateTeamBoardMessageHandler(ButtonFactory buttonFactory)
    {
        this.buttonFactory = buttonFactory;
    }

    protected override async Task<Message> Process(CreateTeamBoardMessageRequest request, CancellationToken cancellationToken)
    {
        Image boardImage = BoardImage.Create(request.Team);

        Button changeTile = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Change tile"),
            () => new ChangeTilesButtonRequest(request.DiscordTeam.Id));

        Button submitEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Submit evidence"),
            () => new SubmitDropButtonRequest(request.DiscordTeam, RSBingo_Framework.Records.EvidenceRecord.EvidenceType.TileVerification,
            Math.Min(General.MaxTilesOnABoard, General.MaxOptionsPerSelectMenu)));

        Button submitDrop = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Submit drop"),
            () => new SubmitDropButtonRequest(request.DiscordTeam, RSBingo_Framework.Records.EvidenceRecord.EvidenceType.Drop, 1));

        Button viewEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "View evidence"),
            () => new ViewEvidenceButtonRequest(request.DiscordTeam.Id));

        string dropCode = string.IsNullOrWhiteSpace(request.Team.Code) ? DropCodeNotSet : request.Team.Code;

        var message = new Message()
            .WithContent(DropCodePrefix.FormatConst(dropCode))
            .AddComponents(changeTile, submitEvidence, submitDrop, viewEvidence)
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