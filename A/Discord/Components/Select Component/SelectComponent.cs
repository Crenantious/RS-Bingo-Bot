// <copyright file="SelectComponent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

using DiscordLibrary.Exceptions;
using DSharpPlus.Entities;

// TODO: JR - add a back button to allow the user to go to the previous page.
// This would likely be a button with a SelectComponentBackButton request that takes in the SelectComponent.
public class SelectComponent : Component<DiscordSelectComponent>, IInteractable
{
    internal List<DiscordSelectComponentOption> DiscordOptions { get; set; } = new();
    internal HashSet<SelectComponentItem> SelectedItemsHashSet { get; set; } = new();
    internal List<SelectComponentItem> SelectedItems { get; set; } = new();
    internal List<SelectComponentPage> SelectedPages { get; } = new();

    public override string Name { get; protected set; }
    //public IReadOnlyList<SelectComponentOption> Options { get; private set; } = null!;
    public bool Disabled { get; }
    public int MinOptions { get; }
    public int MaxOptions { get; }
    public IReadOnlyList<SelectComponentOption> Options => SelectedPages.ElementAt(^1).Options;

    internal SelectComponent(SelectComponentInfo info, string id = "") : base(id)
    {
        this.Disabled = info.Disabled;
        this.MinOptions = info.MinOptions;
        this.MaxOptions = info.MaxOptions;

        Name = $"{info.InitialPage.Label} ({nameof(SelectComponent)})";
    }

    /// <summary>
    /// Validates the component is able to be sent to Discord.
    /// </summary>
    /// <exception cref="SelectComponentNoOptionsException"/>
    public void Validate()
    {
        if (Options.Any() is false)
        {
            throw new SelectComponentNoOptionsException();
        }
    }
}