// <copyright file="RequestGetStats.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.DAL;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using RSBingo_Framework.Scoring;
using RSBingo_Framework.Interfaces;
using DSharpPlus.SlashCommands;
using RSBingoBot.Imaging.Graphs;
using RSBingoBot.DTO;

/// <summary>
/// Request for creating a team.
/// </summary>
public class RequestGetStats : RequestBase
{
    private const string Title = "Score stats";
    private const string LineSeparator = "----------------------------------";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    public override bool IsResponseEphemeral => false;

    public RequestGetStats(InteractionContext ctx, IDataWorker dataWorker) : base(ctx, dataWorker) { }

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();
        IEnumerable<string> stats;

        try
        {
            // TODO: the dw gets saved in CreateTeam and also when the request finishes.
            // Probably make the dw not get auto saved after the request.
            stats = await GetPlayerStats();
        }
        catch (Exception ex) { throw; }
        finally { semaphore.Release(); }

        return ProcessSuccess(MessageUtilities.GetCompiledMessages(stats));
    }

    private protected override bool ValidateSpecificRequest() => true;

    private async Task<IEnumerable<string>> GetPlayerStats()
    {
        IEnumerable<Team> teams = DataWorker.Teams.GetAll().OrderBy(t => t.Score);
        List<string> stats = new();
        stats.Add(Title);

        foreach (Team team in teams)
        {
            stats.Add(LineSeparator);
            stats.Add($"Team {team.Name}: {team.Score}");

            stats.Add("");
            if (team.Users.Any() is false) { stats.Add("No team members."); }

            foreach (User user in team.Users.OrderBy(u => GetScore(u)))
            {
                string username = (await DataFactory.Guild.GetMemberAsync(user.DiscordUserId)).Username;
                stats.Add($"{username}: {GetScore(user)}");
            }
        }

        return stats;
    }

    private int GetScore(User user)
    {
        int score = 0;
        foreach (Evidence evidence in user.Evidence)
        {
            score += Scoring.PointsForDifficulty[evidence.Tile.Task.GetDifficutyAsDifficulty()];
        }
        return score;
    }
}