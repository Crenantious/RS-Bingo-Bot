// <copyright file="SelectComponentGetPageName.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

public class SelectComponentGetPageName
{
    private Func<SelectComponentPage, string> method = null!;

    private SelectComponentGetPageName(Func<SelectComponentPage, string> method)
    {
        this.method = method;
    }

    public static SelectComponentGetPageName FirstToLastOptions(string delimiter = " - ") =>
        new((component) => OnGetPageName(component.Options, delimiter));

    public static SelectComponentGetPageName CustomMethod(Func<SelectComponentPage, string> method) =>
        new(method);

    public string Get(SelectComponentPage page) =>
        method(page);

    private static string OnGetPageName(IReadOnlyList<SelectComponentOption> options, string delimiter) =>
        string.Join(
            delimiter,
            options.ElementAt(0).Label,
            options.ElementAt(options.Count() - 1).Label);
}