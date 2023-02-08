// <copyright file="Leaderboard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Leaderboard;

using DSharpPlus.Entities;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingoBot.Scoring;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static RSBingo_Framework.DAL.DataFactory;

public class Leaderboard
{
    private const string savedImageName = "Leaderboard.png";

    private static IDataWorker dataWorker = CreateDataWorker();
    private static Image leaderboardImage = new Image<Rgba32>(0,0);
    private static Dictionary<Team, int> teamRank = new();

    public static void SetUp() =>
        CreateLeaderboardImage();

    public void UpdateTeam(TeamScore teamScore)
    {
        // Re-creating the board each time is very inefficient, but the system does not need to be fast.
        CreateLeaderboardImage();
    }

    public async Task PostLeaderboard()
    {
        DiscordMessage imageMessage;
        leaderboardImage.SaveAsPng(savedImageName);

        using (var fs = new FileStream(savedImageName, FileMode.Open, FileAccess.Read))
        {
            imageMessage = await DataFactory.LeaderboardChannel.SendMessageAsync(new DiscordMessageBuilder()
                .WithFile("Leaderboard", fs));
        }
    }

    private static void CreateLeaderboardImage()
    {
        IEnumerable<Team> orderedTeams = dataWorker.Teams.GetAll().OrderByDescending(t => Scoring.GetTeamScore(t).Score);
        for (int i = 0; i < orderedTeams.Count(); i++)
        {
            Team team = orderedTeams.ElementAt(i);
            teamRank[team] = i + 1;
            AppendTeamImage(CreateTeamImage(team));
        }
    }

    private static void AppendTeamImage(Image teamImage)
    {
        int imageHeight = leaderboardImage.Height;
        leaderboardImage.Mutate(x => x.Resize(leaderboardImage.Width, imageHeight + LeaderboardTeamBackground.Image.Height));
        leaderboardImage.Mutate(x => x.DrawImage(LeaderboardTeamBackground.Image, new Point(0, imageHeight), 1));
    }

    private static Image CreateTeamImage(Team team)
    {
        Image teamImage = LeaderboardTeamBackground.Image.Clone(x => { });
        DrawTeamInfo(teamImage, team.Name, Scoring.GetTeamScore(team).Score, teamRank[team]);
        return teamImage;
    }

    public static Image DrawTeamInfo(Image background, string name, int score, int rank)
    {
        background.Mutate(x => x.DrawText(name, LeaderboadPreferences.Font,
            LeaderboadPreferences.TextColour, LeaderboardTeamBackground.nameBackground.Centre));

        background.Mutate(x => x.DrawText(score.ToString(), LeaderboadPreferences.Font,
            LeaderboadPreferences.TextColour, LeaderboardTeamBackground.scoreBackground.Centre));

        background.Mutate(x => x.DrawText(rank.ToString(), LeaderboadPreferences.Font,
            LeaderboadPreferences.TextColour, LeaderboardTeamBackground.rankBackground.Centre));

        return background;
    }
}
