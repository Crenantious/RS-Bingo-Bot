// <copyright file="RequestOperateCSVBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.BingoCommands;

using RSBingo_Framework;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.CSV;
using RSBingo_Framework.CSV.Lines;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Exceptions.CSV;
using System.Net;
using Microsoft.Extensions.Logging;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using static RSBingoBot.MessageUtilities;

/// <summary>
/// Request for deleting a team.
/// </summary>
public abstract class RequestOperateCSVBase<LineType> : RequestBase
    where LineType : CSVLine
{
    private const string InvalidFileTypeResponse = "The uploaded file must be a .csv.";
    private const string UnableToDownloadFileResponse = "Unable to download the file. Please try again shortly.";
    private const string ProcessSuccessfulWithWarningsResponse = "{0} but had the following warnings:";
    private const string ProcessSuccessfulWithoutWarningsResponse = "{0}.";
    private const string CsvMediaType = "text/csv";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly DiscordAttachment csvAttachment;

    private ILogger<RequestDeleteTeam> logger = null!;

    protected abstract string ProcessSussessResponse { get; }
    protected string FileName { get; } = Guid.NewGuid().ToString() + ".csv";
    protected CSVData<LineType> Data { get; private set; } = null!;

    public RequestOperateCSVBase(InteractionContext ctx, IDataWorker dataWorker, DiscordAttachment attachment) : base(ctx, dataWorker) =>
        this.csvAttachment = attachment;

    public override async Task<bool> ProcessRequest()
    {
        await semaphore.WaitAsync();
        logger = General.LoggingInstance<RequestDeleteTeam>();
        IEnumerable<string> operatorWarnings;
        WebClient webClient = new();

        try
        {
            webClient.DownloadFile(new Uri(csvAttachment.Url), FileName);
            ParseFile();
            operatorWarnings = Operate();
        }
        catch (WebException e)
        {
            return ProcessFailure(UnableToDownloadFileResponse);
        }
        catch (UnpermittedURLException e)
        {
            return ProcessFailure(GetUnpermittedDomainExceptionMessage(e.Message));
        }
        catch
        {
            throw;
        }
        finally
        {
            semaphore.Release();
        }

        return ProcessSuccess(GetResponseMessages(operatorWarnings));
    }

    private IEnumerable<string> GetResponseMessages(IEnumerable<string> operatorWarnings)
    {
        if (operatorWarnings.Any())
        {
            string messagePrefix = ProcessSuccessfulWithWarningsResponse.FormatConst(ProcessSussessResponse);
            return GetCompiledMessages(operatorWarnings.Prepend(messagePrefix));
        }
        else
        {
            return new string[] { ProcessSuccessfulWithoutWarningsResponse.FormatConst(ProcessSussessResponse) };
        }
    }

    private protected void ParseFile()
    {
        CSVReader reader = new();
        Data = reader.Parse<LineType>(FileName);
    }

    private protected abstract IEnumerable<string> Operate();

    private protected override bool ValidateSpecificRequest()
    {
        if (csvAttachment.MediaType.StartsWith(RequestOperateCSVBase<LineType>.CsvMediaType)) { return true; }
        SetResponseMessage("The uploaded file must be a csv.");
        return false;
    }

    private static IEnumerable<string> GetUnpermittedDomainExceptionMessage(string prefix)
    {
        IEnumerable<string> messages = WhitelistChecker.GetWhitelistedDomains();
        return GetCompiledMessages(messages.Prepend(prefix));
    }
}