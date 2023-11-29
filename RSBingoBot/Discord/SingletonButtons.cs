// <copyright file="SingletonButtons.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.DiscordComponents;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;

public class SingletonButtons
{
    private readonly ButtonFactory buttonFactory;

    public Button CreateTeam { get; private set; }
    public Button JoinTeam { get; private set; }

    public SingletonButtons(ButtonFactory buttonFactory)
    {
        this.buttonFactory = buttonFactory;

        CreateTeam = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Create team", "CreateTeamButton"));
        JoinTeam = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, "Join team", "JoinTeamButton"));
    }
}