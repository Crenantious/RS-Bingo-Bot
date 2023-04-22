// <copyright file="LocalTestServer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

public class LocalTestServer
{
    private const string UriPrefix = "http://localhost:8000/";

    public const string InvalidURL = UriPrefix + "This is an invalid url/";

    private static Dictionary<string, ILocalServerPage> URLToPage = new();
    private static Dictionary<Type, ILocalServerPage> Pages = new();
    private static Task serverTask = null!;
    private static HttpListener listener;
    private static bool isSetup = false;
    private static bool isOpen = false;

    public static void Open()
    {
        if (isOpen) { return; }
        serverTask = RunServer();
        isOpen = true;
    }

    public static void Close() =>
        serverTask?.Dispose();

    public static void Setup()
    {
        if (isSetup) { return; }

        listener = new HttpListener();
        AddPages();
        isSetup = true;
    }

    public static string GetUrl(Type page) =>
        UriPrefix + Pages[page].URL + "/";

    public static string GetUrl<Page>() where Page : ILocalServerPage =>
        GetUrl(typeof(Page));

    private static void AddPages()
    {
        Type pageType = typeof(ILocalServerPage);
        IEnumerable<Type> pages = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Contains(pageType));

        foreach (Type page in pages)
        {
            AddPage(page);
        }
    }

    private static void AddPage(Type pageType)
    {
        ILocalServerPage page = (ILocalServerPage)Activator.CreateInstance(pageType)!;
        Pages.Add(pageType, page);
        string url = GetUrl(pageType);
        listener.Prefixes.Add(url);
        URLToPage.Add(new Uri(url).LocalPath, page);
    }

    private static Task RunServer()
    {
        Setup();
        return Task.Run(() =>
        {
            listener.Start();
            Listen(listener);
        });
    }

    private static void Listen(HttpListener listener)
    {
        HttpListenerContext context = listener.GetContextAsync().Result;
        URLToPage[context.Request.Url.LocalPath].SendResponse(context);
        context.Response.Close();

        Listen(listener);
    }
}