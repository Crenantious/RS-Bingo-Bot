// <copyright file="RequestOperateCSVBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DTO;
using RSBingo_Framework.CSV;
using DSharpPlus.Entities;
using System.Net;

/// <summary>
/// Request for operating on a csv file.
/// </summary>
internal abstract class RequestOperateCSVBase<LineType> : RequestBase
    where LineType : CSVLine
{
    private const string InvalidFileTypeResponse = "The uploaded file must be a .csv.";
    private const string ProcessSuccessfulWithWarnings = "{0} but had the following warnings:";
    private const string ProcessSuccessfulWithoutWarningsResponse = "{0}.";
    private const string CsvMediaType = "text/csv";
    private const string NonCsvFileError = "The uploaded file must be a csv.";

    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly DiscordAttachment csvAttachment;

    protected abstract string ProcessSussessResponse { get; }
    protected string FileName { get; } = Guid.NewGuid().ToString() + ".csv";
    protected CSVData<LineType> Data { get; private set; } = null!;

    public RequestOperateCSVBase(DiscordAttachment attachment) : base(semaphore)
    {
        csvAttachment = attachment;
    }

    protected override bool Validate()
    {
        if (IsCsvFile()) { return true; }
        AddResponse(NonCsvFileError);
        return false;
    }

    protected override async Task Process()
    {
        Result result = WebRequests.DownloadFile(csvAttachment.Url, FileName);
        if (result.IsFaulted)
        {
            AddResponse(result.Error);
            return;
        }

        ParseFile();
        IEnumerable<string> operatorWarnings = Operate();
        SetSuccessResponses(operatorWarnings);
    }

    private bool IsCsvFile() =>
        csvAttachment.MediaType.StartsWith(RequestOperateCSVBase<LineType>.CsvMediaType);

    private protected void ParseFile()
    {
        CSVReader reader = new();
        Data = reader.Parse<LineType>(FileName);
    }

    private protected abstract IEnumerable<string> Operate();

    private void SetSuccessResponses(IEnumerable<string> operatorWarnings)
    {
        if (operatorWarnings.Any())
        {
            AddResponse(ProcessSuccessfulWithWarnings.FormatConst(ProcessSussessResponse));
            AddResponses(operatorWarnings);
        }
        else
        {
            AddResponse(ProcessSuccessfulWithoutWarningsResponse.FormatConst(ProcessSussessResponse));
        }
    }
}