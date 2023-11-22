// <copyright file="SelectComponentPageName.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

public class SelectComponentPageName
{
    private Func<SelectComponentPage, string> method = null!;

    private SelectComponentPageName(Func<SelectComponentPage, string> method)
    {
        this.method = method;
    }

    public static SelectComponentPageName FirstToLastOptions(string delimiter = " - ") =>
        new((component) => OnGetPageName(component.Options, delimiter));

    public static SelectComponentPageName CustomMethod(Func<SelectComponentPage, string> method) =>
        new(method);

    public string Get(SelectComponentPage selectComponent) =>
        method(selectComponent);

    private static string OnGetPageName(IReadOnlyList<SelectComponentOption> options, string delimiter) =>
        string.Join(
            options.ElementAt(0).label,
            options.ElementAt(options.Count() - 1).label,
            delimiter);
}