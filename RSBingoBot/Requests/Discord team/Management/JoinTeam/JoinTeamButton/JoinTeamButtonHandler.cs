// <copyright file="JoinTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using RSBingoBot.Discord;

internal class JoinTeamButtonHandler : ButtonHandler<JoinTeamButtonRequest>
{
    private const string SelectATeamMessage = "{0} Select a team to join";
    private const string NoTeamsExistMessage = "{0} No teams exists yet.";

    private readonly SelectComponentFactory selectComponentFactory;
    private readonly ButtonFactory buttonFactory;

    private InteractionMessage response;

    protected override bool SendKeepAliveMessageIsEphemeral => false;

    public JoinTeamButtonHandler(SelectComponentFactory selectComponentFactory, ButtonFactory buttonFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
        this.buttonFactory = buttonFactory;
    }

    protected override async Task Process(JoinTeamButtonRequest request, CancellationToken cancellationToken)
    {
        var messageService = GetRequestService<IDiscordInteractionMessagingServices>();
        response = new InteractionMessage(request.GetDiscordInteraction());

        if (DiscordTeam.ExistingTeams.Any())
        {
            response.WithContent(FormatContent(SelectATeamMessage))
                .AddComponents(GetSelectComponent(request));
        }
        else
        {
            response.WithContent(FormatContent(NoTeamsExistMessage));
        }

        response.AddComponents(GetCloseButton());

        await messageService.Send(response);
    }

    private string FormatContent(string message) =>
        message.FormatConst(Interaction.User.Mention);

    private SelectComponent GetSelectComponent(JoinTeamButtonRequest request) =>
        selectComponentFactory.Create(new SelectComponentInfo(new SelectComponentPage("Select a team", GetSelectOptions())),
            () => new JoinTeamSelectRequest(request.GetDiscordInteraction().User));

    // TODO: JR - add SelectItemsGenerator to make this simple since it's commonplace.
    // I.e. a method: FromEnumerable(IEnumerable<T>, Func<T, SelectComponentItem>)
    private IEnumerable<SelectComponentItem> GetSelectOptions()
    {
        var teams = DiscordTeam.ExistingTeams.OrderBy(kvp => kvp.Key);
        return teams.Select(t => new SelectComponentItem(t.Key, t.Value));
    }

    private Button GetCloseButton() =>
        buttonFactory.CreateConcludeInteraction(new(InteractionTracker, new List<Message>() { response }, Interaction.User));
}