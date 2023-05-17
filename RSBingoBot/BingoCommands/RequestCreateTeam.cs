// <copyright file="RequestCreateTeam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework.Interfaces;
using DSharpPlus.SlashCommands;

/// <summary>
/// Request for creating a team.
/// </summary>
public class RequestCreateTeam : RequestBase
{
    private const string TeamSuccessfullyCreatedMessage = "The team '{0}' has been created successfully.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly string teamName;

    public RequestCreateTeam(InteractionContext ctx, IDataWorker dataWorker, string teamName) : base(ctx, dataWorker) =>
        this.teamName = teamName;

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();
        await CreateTeam();
        semaphore.Release();
        return ProcessSuccess(TeamSuccessfullyCreatedMessage.FormatConst(teamName));
    }

    private protected override bool ValidateSpecificRequest()
    {
        IEnumerable<string> errors = RequestsUtilities.GetNewTeamNameErrors(teamName, DataWorker);
        SetResponseMessage(MessageUtilities.GetCompiledMessages(errors));
        return errors.Any() is false;
    }

    private async Task CreateTeam() =>
         await new RSBingoBot.DiscordTeam(Ctx.Client, teamName).InitialiseAsync();
}