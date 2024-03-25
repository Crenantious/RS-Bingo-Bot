// <copyright file="EvidenceReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using DSharpPlus.Entities;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;
using DiscordTeam = Discord.DiscordTeam;

// TODO: JR - refactor as this does too much. Move a lot of the logic into specialised classes.
internal abstract class EvidenceReactionHandler<TRequest> : RequestHandler<TRequest> where TRequest : EvidenceReactionRequest
{
    private readonly MessageFactory messageFactory;

    private IDataWorker dataWorker = null!;
    private IDatabaseServices dbServices = null!;
    private IScoringServices scoringServices = null!;
    private IDiscordMessageServices messageServices = null!;
    private IDiscordTeamServices teamServices = null!;
    private Message evidenceMessage = null!;

    protected abstract DiscordChannel ChannelToMoveEvidenceTo { get; set; }
    protected abstract EvidenceStatus NewEvidenceStatus { get; set; }

    public EvidenceReactionHandler()
    {
        this.messageFactory = General.DI.Get<MessageFactory>();
    }

    protected abstract bool ValidateEvidence(Evidence evidence);

    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        dataWorker = DataFactory.CreateDataWorker();
        evidenceMessage = request.GetMessage();
        Evidence? evidence = dataWorker.Evidence.FirstOrDefault(e => e.DiscordMessageId == evidenceMessage.DiscordMessage.Id);

        // This isn't done in the validator to avoid searching the db multiple times.
        // We don't add an error here as we're just ignoring the case.
        if (evidence is null || ValidateEvidence(evidence) is false)
        {
            return;
        }

        GetServices();

        if (await HandleEvidence(evidence) is false)
        {
            return;
        }

        Team team = evidence.Tile.Team;
        DiscordTeam discordTeam = DiscordTeam.ExistingTeams[team.Name];
        int[] boardIndexes = new int[] { evidence.Tile.BoardIndex };

        UpdateScore(discordTeam, team);
        await teamServices.UpdateBoardImage(discordTeam, team, boardIndexes);
    }

    private void GetServices()
    {
        messageServices = GetRequestService<IDiscordMessageServices>();
        scoringServices = GetRequestService<IScoringServices>();
        dbServices = GetRequestService<IDatabaseServices>();
        teamServices = GetRequestService<IDiscordTeamServices>();
    }

    private async Task<bool> HandleEvidence(Evidence evidence)
    {
        var verifiedMessage = await SendEvidenceMessageToChannel(ChannelToMoveEvidenceTo);
        if (verifiedMessage.IsFailed)
        {
            return false;
        }

        if (await UpdateDB(verifiedMessage.Value, evidence, NewEvidenceStatus) is false)
        {
            return false;
        }

        await messageServices.Delete(evidenceMessage);
        return true;
    }

    private async Task<Result<Message>> SendEvidenceMessageToChannel(DiscordChannel channel)
    {
        var webServices = GetRequestService<IWebServices>();

        Message newMessage = await messageFactory.Create(evidenceMessage.DiscordMessage, webServices);
        newMessage.Channel = channel;
        var result = await messageServices.Send(newMessage);

        return new Result<Message>()
            .WithValue(newMessage)
            .WithErrors(result.Errors);
    }

    private async Task<bool> UpdateDB(Message message, Evidence evidence, EvidenceStatus evidenceStatus)
    {
        evidence.Status = EvidenceStatusLookup.Get(evidenceStatus);
        evidence.DiscordMessageId = message.DiscordMessage.Id;
        evidence.Tile.UpdateVerifiedStatus();
        evidence.Tile.UpdateCompleteStatus();

        var result = await dbServices.SaveChanges(dataWorker);
        return result.IsSuccess;
    }

    private void UpdateScore(DiscordTeam discordTeam, Team team)
    {
        scoringServices.UpdateTeam(discordTeam, team);
        scoringServices.UpdateLeaderboard();
    }
}