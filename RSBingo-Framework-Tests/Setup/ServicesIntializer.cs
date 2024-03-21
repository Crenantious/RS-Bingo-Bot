// <copyright file="ServicesIntializer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static RSBingo_Common.General;

[TestClass]
public class ServicesIntializer
{
    private static IConfigurationRoot configurationRoot;

    static ServicesIntializer()
    {
        ConfigurationBuilder configuration = new();
        configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), false);
        configuration.AddUserSecrets(typeof(ServicesIntializer).Assembly, false);
        configurationRoot = configuration.Build();
    }

    [TestInitialize]
    public virtual void TestInitialize()
    {
        ServiceCollection services = new();
        services.AddLogging(b => b.AddConsole());
        AddServices(services);

        ContainerBuilder builder = new();
        builder.Populate(services);

        builder.RegisterInstance(configurationRoot).As<IConfiguration>();

        DI = new AutofacServiceProvider(builder.Build());
    }

    protected virtual void AddServices(ServiceCollection services)
    {

    }
}