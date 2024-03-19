// <copyright file="UserHasNoAcceptedVerificationEvidenceForTileDPTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DataParsers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RSBingo_Common;
using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;
using RSBingoBot.Requests;

[TestClass]
public class UserHasTheOnlyPendingDropsTSVTests : TSVTestBase
{
    private UserHasTheOnlyPendingDropsTSV tsv = null!;
    private Mock<IUserHasTheOnlyPendingDropsDP> parser = new();
    private bool? isValid = null;

    protected override void AddServices(ServiceCollection services)
    {
        base.AddServices(services);

        services.AddSingleton<IUserHasTheOnlyPendingDropsDP>(s => parser.Object);
        services.AddTransient<UserHasTheOnlyPendingDropsTSV>();
    }

    [TestInitialize]
    public override void TestInitialize()
    {
        base.TestInitialize();

        UserOne = MockDBSetup.Add_User(DataWorker, 0, Team);

        tsv = General.DI.Get<UserHasTheOnlyPendingDropsTSV>();
    }

    [TestMethod]
    public void ParserHasNoEvidence_TSVIsValid()
    {
        SetEvidence();

        Validate();

        AssertValidation(true);
    }

    [TestMethod]
    [DataRow(EvidenceEnum.PendingVerification)]
    [DataRow(EvidenceEnum.RejectedVerification)]
    [DataRow(EvidenceEnum.AcceptedVerification)]
    [DataRow(EvidenceEnum.PendingDrop)]
    [DataRow(EvidenceEnum.RejectedDrop)]
    [DataRow(EvidenceEnum.AcceptedDrop)]
    public void ParserHasVariedEvidence_TSVIsInValid(EvidenceEnum userOneEvidence)
    {
        var evidence = AddEvidence(TileOne, UserOne, userOneEvidence);
        SetEvidence(evidence);

        Validate();

        AssertValidation(false);
    }

    private void SetEvidence(params Evidence[] evidence)
    {
        SetEvidence(evidence.ToList());
    }

    private void SetEvidence(IEnumerable<Evidence> evidence)
    {
        parser.Setup(p => p.Evidence).Returns(evidence);
    }

    private void Validate()
    {
        // We don't care about the parameters because we've mocked
        // the parser that contains the data to be used.
        isValid = tsv.Validate(null!, null!);
    }

    private void AssertValidation(bool expected)
    {
        if (isValid is null)
        {
            throw new InvalidOperationException($"Must call {nameof(Validate)} before {nameof(AssertValidation)}.");
        }

        Assert.AreEqual(expected, isValid);
    }
}