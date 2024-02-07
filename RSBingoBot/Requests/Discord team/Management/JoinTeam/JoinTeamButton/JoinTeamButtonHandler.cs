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
    private const string SelectMessage = "Select a team to join";

    private readonly SelectComponentFactory selectComponentFactory;

    public JoinTeamButtonHandler(SelectComponentFactory selectComponentFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
    }

    protected override async Task Process(JoinTeamButtonRequest request, CancellationToken cancellationToken)
    {
        var messageService = GetRequestService<IDiscordInteractionMessagingServices>();

        var response = new InteractionMessage(request.GetDiscordInteraction())
            .WithContent(SelectMessage)
            .AddComponents(GetSelectComponent(request));

        await messageService.Send(response);
    }

    private SelectComponent GetSelectComponent(JoinTeamButtonRequest request)
    {
        var user = request.GetDiscordInteraction().User;
        return selectComponentFactory.Create(new("Select a team", GetSelectOptions()), () => new JoinTeamSelectRequest(user));
    }

    // TODO: JR - add SelectItemsGenerator to make this simple since it's commonplace.
    // I.e. a method: FromEnumerable(IEnumerable<T>, Func<T, SelectComponentItem>)
    private IEnumerable<SelectComponentItem> GetSelectOptions()
    {
        var teams = DiscordTeam.ExistingTeams.OrderBy(kvp => kvp.Key);
        return teams.Select(t => new SelectComponentItem(t.Key, t.Value));
    }
}