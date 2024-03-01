// <copyright file="EvidenceVerificationReactionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordEntities;
using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using DiscordLibrary.Requests.Extensions;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.Records;
using static RSBingo_Framework.Records.EvidenceRecord;

internal class EvidenceVerificationReactionHandler : RequestHandler<EvidenceVerificationReactionRequest>
{
    private IDiscordMessageServices messageServices = null!;
    private IDatabaseServices dbServices = null!;

    protected override async Task Process(EvidenceVerificationReactionRequest request, CancellationToken cancellationToken)
    {
        messageServices = GetRequestService<IDiscordMessageServices>();
        dbServices = GetRequestService<IDatabaseServices>();
        IDataWorker dataWorker = DataFactory.CreateDataWorker();

        Message evidenceMessage = request.GetMessage();
        Evidence? evidence = dataWorker.Evidence.FirstOrDefault(e => e.DiscordMessageId == evidenceMessage.DiscordMessage.Id);

        if (evidence is null || evidence.IsVerified())
        {
            // This isn't done in the validator to avoid searching the db multiple times.
            return;
        }

        var verifiedMessage = await SendEvidenceToVerifiedChannel(evidenceMessage);
        if (verifiedMessage.IsFailed)
        {
            // There may be some failure other than the message sending.
            await messageServices.Delete(verifiedMessage.Value);
            return;
        }

        if (await SaveDBChanges(dataWorker, verifiedMessage.Value, evidence) is false)
        {
            return;
        }

        await messageServices.Delete(evidenceMessage);

        AddSuccess(new EvidenceVerificationReactionSuccess());
    }

    private async Task<Result<Message>> SendEvidenceToVerifiedChannel(Message message)
    {
        Message newMessage = new(message.DiscordMessage);
        newMessage.Channel = DataFactory.VerifiedEvidenceChannel;
        var result = await messageServices.Send(newMessage);

        return new Result<Message>()
            .WithValue(newMessage)
            .WithErrors(result.Errors);
    }

    private async Task<bool> SaveDBChanges(IDataWorker dataWorker, Message message, Evidence evidence)
    {
        evidence.Status = EvidenceStatusLookup.Get(EvidenceStatus.Accepted);
        evidence.DiscordMessageId = message.DiscordMessage.Id;

        var result = await dbServices.SaveChanges(dataWorker);
        return result.IsSuccess;
    }
}