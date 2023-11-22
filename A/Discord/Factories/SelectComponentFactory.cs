// <copyright file="SelectComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordComponents;
using Requests;

public class SelectComponentFactory : InteractableComponentFactory<SelectComponentInfo, SelectComponent, ISelectComponentRequest>
{
    protected internal override SelectComponent Create(SelectComponentInfo createInfo)
    {
        SelectComponent selectComponent = new(createInfo);
        SelectComponentUpdater.Build(selectComponent);
        return selectComponent;
    }
}