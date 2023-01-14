// <copyright file="SelectComponentTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests;

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Component_interaction_handlers.Select_Component;
using static RSBingo_Common.General;

[TestClass]
public class SelectComponentTests : MockDBBaseTestClass
{
    SelectComponent selectComponent = null!;

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        IDataWorker dataWorkerBefore = CreateDW();
        selectComponent = new("", "", (args) => { return null!; });
    }

    [TestMethod]
    public void AddMoreItemsToSelectComponentThanMax_SplitIntoPages()
    {
        List<SelectComponentItem> items = CreateItems(MaxOptionsPerSelectMenu*2 + 1);
        selectComponent.Options = new(items);

        selectComponent.Build();

        Assert.AreEqual(selectComponent.Options.Count, 3);
        Assert.AreEqual(selectComponent.Options[0].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(selectComponent.Options[1].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(selectComponent.Options[2].GetType(), typeof(SelectComponentPage));
    }

    [TestMethod]
    public void AddMoreItemsToPageThanMax_SplitIntoPages()
    {
        SelectComponentPage page = new("");
        List<SelectComponentItem> items = CreateItems(MaxOptionsPerSelectMenu*2 + 1);
        page.Options = new(items);
        selectComponent.Options.Add(page);

        selectComponent.Build();

        Assert.AreEqual(page.Options.Count, 3);
        Assert.AreEqual(page.Options[0].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(page.Options[1].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(page.Options[2].GetType(), typeof(SelectComponentPage));
    }
    
    [TestMethod]
    public void AddMoreItemsToPageInPageThanMax_SplitIntoPages()
    {
        SelectComponentPage page1 = new("");
        SelectComponentPage page2 = new("");
        List<SelectComponentItem> items = CreateItems(MaxOptionsPerSelectMenu * 2 + 1);
        page1.Options = new(items);
        page2.Options = new() { page1 };
        selectComponent.Options.Add(page2);

        selectComponent.Build();

        Assert.AreEqual(page1.Options.Count, 3);
        Assert.AreEqual(page1.Options[0].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(page1.Options[1].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(page1.Options[2].GetType(), typeof(SelectComponentPage));
        Assert.AreEqual(page2.Options.Count, 1);
        Assert.AreEqual(page2.Options[0].GetType(), typeof(SelectComponentPage));
    }

    [TestMethod]
    public void AddLessItemsToSelectComponentThanMax_DontSplitIntoPages()
    {
        SelectComponentPage page = new("");
        int numberOfItems = 5;
        List<SelectComponentItem> items = CreateItems(numberOfItems);
        page.Options = new(items);
        selectComponent.Options.Add(page);

        selectComponent.Build();

        Assert.AreEqual(page.Options.Count, numberOfItems);
        for (int i = 0; i < numberOfItems; i++)
        {
            Assert.AreEqual(page.Options[i].GetType(), typeof(SelectComponentItem));
        }
    }

    [TestMethod]
    public void AddLessItemsToPageThanMax_DontSplitIntoPages()
    {
        SelectComponentPage page = new("");
        int numberOfItems = 5;
        List<SelectComponentItem> items = CreateItems(numberOfItems);
        page.Options = new(items);
        selectComponent.Options.Add(page);

        selectComponent.Build();

        Assert.AreEqual(page.Options.Count, numberOfItems);
        for (int i = 0; i < numberOfItems; i++)
        {
            Assert.AreEqual(page.Options[i].GetType(), typeof(SelectComponentItem));
        }
    }

    private List<SelectComponentItem> CreateItems(int amount)
    {
        List<SelectComponentItem> items = new();
        for (int i = 0; i < amount; i++)
        {
            items.Add(new("", null));
        }
        return items;
    }
}
