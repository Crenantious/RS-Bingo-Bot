// <copyright file="SelectComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.DiscordEventHandlers;
using DiscordLibrary.Interactions;
using DiscordLibrary.Requests;

public static class SelectComponentFactory
{
    public static SelectComponent Create(SelectComponentInfo info, ISelectComponentRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        SelectComponent selectComponent = new(info);

        request.SelectComponent = selectComponent;
        request.SelectComponent.CustomId = Guid.NewGuid().ToString();

        selectComponent.Build();
        selectComponent.Register(request, constraints);

        return selectComponent;
    }
}