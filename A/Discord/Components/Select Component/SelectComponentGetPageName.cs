// <copyright file="SelectComponentGetPageName.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DiscordLibrary.DiscordComponents;

public class SelectComponentGetPageName
{
    private Func<ISelectComponentPage, string> method = null!;

    private SelectComponentGetPageName(Func<ISelectComponentPage, string> method)
    {
        this.method = method;
    }

    public static SelectComponentGetPageName FirstToLastOptions(string delimiter = " - ") =>
        new((component) => OnGetPageName(component.Options, delimiter));

    public static SelectComponentGetPageName CustomMethod(Func<ISelectComponentPage, string> method) =>
        new(method);

    public string Get(ISelectComponentPage page) =>
        method(page);

    private static string OnGetPageName(IReadOnlyList<SelectComponentOption> options, string delimiter) =>
        string.Join(
            delimiter,
            options.ElementAt(0).Label,
            options.ElementAt(options.Count() - 1).Label);
}