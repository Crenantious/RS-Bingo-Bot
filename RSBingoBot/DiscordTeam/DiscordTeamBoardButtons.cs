// <copyright file="DiscordTeamBoardButtons.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Discord;

using DiscordLibrary.DiscordComponents;
using DiscordLibrary.Factories;
using RSBingoBot.Requests;

public class DiscordTeamBoardButtons
{
    private readonly ButtonFactory buttonFactory;

    public const string ChangeTileLabel = "Change tile";
    public const string SubmitEvidenceLabel = "Submit evidence";
    public const string SubmitDropLabel = "Submit drop";
    public const string ViewEvidenceLabel = "View evidence";

    /// <summary>
    /// Must call <see cref="Create(DiscordTeam)"/> to set this value.
    /// </summary>
    public Button ChangeTile { get; private set; }

    /// <inheritdoc cref="ChangeTile"/>
    public Button SubmitEvidence { get; private set; }

    /// <inheritdoc cref="ChangeTile"/>
    public Button SubmitDrop { get; private set; }

    /// <inheritdoc cref="ChangeTile"/>
    public Button ViewEvidence { get; private set; }

    public DiscordTeamBoardButtons(ButtonFactory buttonFactory)
    {
        this.buttonFactory = buttonFactory;
    }

    public void Create(DiscordTeam team)
    {
        ChangeTile = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, ChangeTileLabel, GetId(team, ChangeTileLabel)),
            () => new ChangeTilesButtonRequest(team.Id));

        SubmitEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, SubmitEvidenceLabel, GetId(team, SubmitEvidenceLabel)),
            () => new SubmitEvidenceButtonRequest(team, RSBingo_Framework.Records.EvidenceRecord.EvidenceType.TileVerification,
            Math.Min(General.MaxTilesOnABoard, General.MaxSelectOptionsPerPage)));

        SubmitDrop = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, SubmitDropLabel, GetId(team, SubmitDropLabel)),
            () => new SubmitEvidenceButtonRequest(team, RSBingo_Framework.Records.EvidenceRecord.EvidenceType.Drop, 1));

        ViewEvidence = buttonFactory.Create(new(DSharpPlus.ButtonStyle.Primary, ViewEvidenceLabel, GetId(team, ViewEvidenceLabel)),
            () => new ViewEvidenceButtonRequest(team.Id));
    }

    private string GetId(DiscordTeam team, string buttonName) =>
        $"team {team.Name} {buttonName}";
}