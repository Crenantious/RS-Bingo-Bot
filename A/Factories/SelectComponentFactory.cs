// <copyright file="SelectComponentFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.Factories;

using DiscordComponents;
using Requests;

public class SelectComponentFactory : ComponentFactory<SelectComponentInfo, ISelectComponentRequest>
{
    protected internal override IDiscordComponent Create(SelectComponentInfo createInfo, MetaData metaData)
    {
        SelectComponent selectComponent = new(createInfo);
        SelectComponentUpdater.Build(selectComponent);
        return selectComponent;
    }
}