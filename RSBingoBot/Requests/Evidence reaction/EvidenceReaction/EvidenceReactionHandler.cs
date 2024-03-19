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

internal class EvidenceReactionHandler<TRequest> : RequestHandler<TRequest> where TRequest : EvidenceReactionRequest
{
    private readonly MessageFactory messageFactory;

    private IDiscordMessageServices messageServices = null!;
    private IDatabaseServices dbServices = null!;
    private IScoringServices scoringServices = null!;
    private IDataWorker dataWorker = null!;

    protected Evidence? Evidence { get; private set; }
    protected Message EvidenceMessage { get; private set; } = null!;

    public EvidenceReactionHandler()
    {
        this.messageFactory = General.DI.Get<MessageFactory>();
    }

    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        messageServices = GetRequestService<IDiscordMessageServices>();
        scoringServices = GetRequestService<IScoringServices>();
        dbServices = GetRequestService<IDatabaseServices>();
        dataWorker = DataFactory.CreateDataWorker();

        EvidenceMessage = request.GetMessage();
        Evidence = dataWorker.Evidence.FirstOrDefault(e => e.DiscordMessageId == EvidenceMessage.DiscordMessage.Id);
    }

    /// <summary>
    /// Moves the message that was reacted to and updates the database.
    /// </summary>
    /// <param name="evidenceStatus">What the new status of the evidence should be after the reaction.</param>
    protected async Task MoveEvidenceMessage(Evidence evidence, DiscordChannel channel, EvidenceStatus evidenceStatus)
    {
        var verifiedMessage = await SendEvidenceMessageToChannel(channel);
        if (verifiedMessage.IsFailed)
        {
            return;
        }

        if (await SaveDBChanges(verifiedMessage.Value, evidence, evidenceStatus) is false)
        {
            return;
        }

        await messageServices.Delete(EvidenceMessage);
    }

    private async Task<Result<Message>> SendEvidenceMessageToChannel(DiscordChannel channel)
    {
        var webServices = GetRequestService<IWebServices>();

        Message newMessage = await messageFactory.Create(EvidenceMessage.DiscordMessage, webServices);
        newMessage.Channel = channel;
        var result = await messageServices.Send(newMessage);

        return new Result<Message>()
            .WithValue(newMessage)
            .WithErrors(result.Errors);
    }

    private async Task<bool> SaveDBChanges(Message message, Evidence evidence, EvidenceStatus evidenceStatus)
    {
        evidence.Status = EvidenceStatusLookup.Get(evidenceStatus);
        evidence.DiscordMessageId = message.DiscordMessage.Id;

        if (evidence.IsType(EvidenceType.Drop))
        {
            var completeStatus = evidenceStatus == EvidenceStatus.Accepted ?
                                 TileRecord.CompleteStatus.Yes :
                                 TileRecord.CompleteStatus.No;

            evidence.Tile.SetCompleteStatus(completeStatus);
        }

        var result = await dbServices.SaveChanges(dataWorker);
        return result.IsSuccess;
    }

    protected void UpdateScore()
    {
        Team team = Evidence!.Tile.Team;
        scoringServices.UpdateTeam(DiscordTeam.ExistingTeams[team.Name], team);
        scoringServices.UpdateLeaderboard();
    }
}