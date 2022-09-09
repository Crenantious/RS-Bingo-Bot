// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BingoBotEmbed;

using Microsoft.Extensions.Configuration;
using RSBingo_Framework.DAL;
using static RSBingo_Common.General;

/// <summary>
/// Entry point to the bot.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point the the program
    /// </summary>
    /// <param name="args">Argument array.</param>
    public static void Main(string[] args)
    {
        LoggingStart();

        // Configure the app to add User secrets and appsetting which we can access as a service.
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json") // Currently not used.
            .AddUserSecrets<Program>()
            .Build();

        // TODO: JCH - Assign Common.General.DI to required services. Not sure where we will get these from as they are tridionally are from WebAppBuilder.Services.

        // Tell the DataFactory we want it to create connections in default mode
        DataFactory.SetupDataFactory();

        Bot bot = new ();
        bot.RunAsync().GetAwaiter().GetResult();
    }
}