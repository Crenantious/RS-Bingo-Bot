// <copyright file="JoinTeamButtonHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEntities;
using DiscordLibrary.Factories;
using DiscordLibrary.Requests;
using RSBingo_Framework.Models;

internal class JoinTeamButtonHandler : ButtonHandler<JoinTeamButtonRequest>
{
    private readonly SelectComponentFactory selectComponentFactory;

    public JoinTeamButtonHandler(SelectComponentFactory selectComponentFactory)
    {
        this.selectComponentFactory = selectComponentFactory;
    }

    protected override async Task Process(JoinTeamButtonRequest request, CancellationToken cancellationToken)
    {
        SelectComponent selectComponent = selectComponentFactory.Create(new("Select a team", GetSelectOptions()),
                                                                        new JoinTeamSelectRequest(request.User));
        var response = new InteractionMessage(InteractionArgs.Interaction)
                           .AddComponents(selectComponent)
                           .AsEphemeral(true);
        ResponseMessages.Add(response);
    }

    // TODO: JR - add SelectItemsGenerator to make this simple since it's commonplace.
    // I.e. a method: FromEnumerable(IEnumerable<T>, Func<T, SelectComponentItem>)
    private List<SelectComponentItem> GetSelectOptions()
    {
        List<SelectComponentItem> items = new();
        foreach (Team team in DataWorker.Teams.GetAll())
        {
            items.Add(new(team.Name, team));
        }
        return items;
    }
}