// <copyright file="ChangeTilesButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordExtensions;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;

internal class ChangeTilesButtonHandler : ButtonHandler<ChangeTilesButtonRequest>
{
    private const string ResponseContent = "{0} Select tiles to swap. The 'Change from' selection contains the team's tiles, " +
        "the 'Change to' selection contains all possible tiles. A tick in the 'Change to' selection indicates the tile is on the board.";

    private readonly ButtonFactory buttonFactory;
    private readonly SelectComponentFactory selectFactory;

    private User user = null!;

    protected override bool SendKeepAliveMessage => false;

    public ChangeTilesButtonHandler(ButtonFactory buttonFactory, SelectComponentFactory selectFactory)
    {
        this.buttonFactory = buttonFactory;
        this.selectFactory = selectFactory;
    }

    protected override async Task Process(ChangeTilesButtonRequest request, CancellationToken cancellationToken)
    {
        IDataWorker dataWorker = DataFactory.CreateDataWorker();
        var messageServices = GetRequestService<IDiscordInteractionMessagingServices>();

        await messageServices.SendKeepAlive(Interaction, false);

        user = Interaction.User.GetDBUser(dataWorker)!;

        var response = new InteractionMessage(Interaction)
            .WithContent(GetResponseContent(request));

        ChangeTilesButtonDTO dto = new();

        ChangeTilesTileSelect tileSelect = new(user.Team, dto);
        ChangeTilesTaskSelect taskSelect = new(dataWorker, user.Team, dto);

        Button changeFromBack = buttonFactory.CreateSelectComponentBackButton(() => new(tileSelect.SelectComponent));
        Button changeToBack = buttonFactory.CreateSelectComponentBackButton(() => new(taskSelect.SelectComponent));
        Button apply = buttonFactory.Create(new(ButtonStyle.Primary, "Apply"),
            () => new ChangeTilesSubmitButtonRequest(dataWorker, user.Team, dto, Interaction.User, tileSelect, taskSelect));
        Button close = buttonFactory.CreateConcludeInteraction(() => new(InteractionTracker, new List<Message>() { response }));

        response.AddComponents(tileSelect.SelectComponent)
            .AddComponents(changeFromBack)
            .AddComponents(taskSelect.SelectComponent)
            .AddComponents(changeToBack)
            .AddComponents(apply, close);

        await messageServices.Send(response);
    }

    private static string GetResponseContent(ChangeTilesButtonRequest request) =>
        ResponseContent.FormatConst(request.GetDiscordInteraction().User.Mention);
}