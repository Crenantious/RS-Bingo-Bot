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
    private static HashSet<Type> typesWithInisialisedServices = new();

    [TestInitialize]
    public virtual void TestInitialize()
    {
        if (typesWithInisialisedServices.Contains(GetType()))
        {
            return;
        }

        ServiceCollection services = new();
        services.AddLogging(b => b.AddConsole());
        AddServices(services);

        ContainerBuilder builder = new();
        builder.Populate(services);

        ConfigurationBuilder configuration = new();
        configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), false);
        configuration.AddUserSecrets(typeof(ServicesIntializer).Assembly, false);

        builder.RegisterInstance(configuration.Build()).As<IConfiguration>();

        DI = new AutofacServiceProvider(builder.Build());

        typesWithInisialisedServices.Add(GetType());
    }

    protected virtual void AddServices(ServiceCollection services)
    {

    }
}