// <copyright file="AssemblyInitializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using RSBingo_Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using static RSBingo_Common.General;

[TestClass]
public static class AssemblyInitializer
{
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context)
    {
        context.WriteLine("Building DI/DB");
        ServiceCollection services = new ();
        services.AddLogging(b => b.AddConsole());

        ContainerBuilder builder = new ();
        builder.Populate(services);

        ConfigurationBuilder configuration = new ();
        configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), false);
        configuration.AddUserSecrets(typeof(AssemblyInitializer).Assembly, false);

        builder.RegisterInstance(configuration.Build()).As<IConfiguration>();

        DI = new AutofacServiceProvider(builder.Build());

        MockDBSetup.SetupDataFactory();

        Paths.Initialise(true);
    }
}
