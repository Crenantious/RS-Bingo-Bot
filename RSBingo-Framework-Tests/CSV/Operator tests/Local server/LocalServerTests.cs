// <copyright file="LocalServerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.CSV.LocalServer;

using System.Net;

[TestClass]
public class LocalServerTests : MockDBBaseTestClass
{
    private WebClient client;

    static LocalServerTests() =>
        LocalTestServer.Open();

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();
        client = new();
    }

    [TestCleanup]
    public override void TestCleanup() =>
        client.Dispose();

    [TestMethod]
    // This method is to test the files are downloaded correctly via manual inspection.
    public void DownloadFileFromServerPage_NoExceptionsOccur()
    {
        DownloadFile<ValidImagePage>();
        DownloadFile<CorruptImagePage>();
        DownloadFile<InvalidImageFormatPage>();

        // Since the test is for manual inspection, this is to avoid confusion when running all tests.
        Assert.IsTrue(true);
    }

    private void DownloadFile<Page>() =>
        client.DownloadFile(LocalTestServer.GetUrl(typeof(Page)), $"Downloaded test image from {nameof(Page)}.png");
}