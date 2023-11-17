// <copyright file="ButtonFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Requests;

// TODO: JR - allow button to be created by a group style. Could be achieved by having
// a sub factory for each one (team registration button, board button etc.), or perhaps
// with a static class containing the groups as ButtonTemplates that are referenced and
// passed in here (or to a sister class).
public class ButtonFactory : ComponentFactory<ButtonInfo, IButtonRequest>
{
    protected internal override IDiscordComponent Create(ButtonInfo buttonInfo, MetaData metaData) =>
        new Button(buttonInfo);
}