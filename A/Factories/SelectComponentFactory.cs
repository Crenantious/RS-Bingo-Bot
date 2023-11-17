// <copyright file="SelectComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordComponents;
using DiscordEventHandlers;
using Interactions;
using Requests;

public static class SelectComponentFactory
{
    public static SelectComponent Create(SelectComponentInfo info, ISelectComponentRequest request,
        ComponentInteractionDEH.StrippedConstraints constraints)
    {
        SelectComponent selectComponent = new(info);
        SelectComponentUpdater.Build(selectComponent);

        MetaData metaData = new();
        metaData.Add<SelectComponent>(selectComponent);

        selectComponent.Register(request, constraints, metaData);

        return selectComponent;
    }
}