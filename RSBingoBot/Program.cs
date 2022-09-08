// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BingoBotEmbed;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Entry point to the bot.
/// </summary>
public class Program
{
    static void Main(string[] args)
    {

        // Configure the app to add User secrets and appsetting which we can access as a service.
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json") // Currently not used.
            .AddUserSecrets<Program>()
            .Build();

        Bot bot = new ();
        bot.RunAsync().GetAwaiter().GetResult();
    }
}