// <copyright file="OperateCSVHandlerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using FluentResults;
using RSBingo_Framework.CSV;

internal abstract class OperateCSVHandlerBase<LineType> : RequestHandler<OperateCSVRequest>
    where LineType : CSVLine
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    protected abstract string SuccessResponse { get; }
    protected string FileName { get; } = Guid.NewGuid().ToString() + ".csv";
    protected CSVData<LineType> Data { get; private set; } = null!;

    public OperateCSVHandlerBase() : base(semaphore)
    {

    }

    protected override async Task Process(OperateCSVRequest request, CancellationToken cancellationToken)
    {
        Result result = WebRequests.DownloadFile(request.Attachment.Url, FileName);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
            return;
        }

        AddSuccess(SuccessResponse);

        ParseFile();
        IEnumerable<string> warnings = Operate();
        SetWarnings(warnings);
    }

    private protected void ParseFile()
    {
        CSVReader reader = new();
        Data = reader.Parse<LineType>(FileName);
    }

    private protected abstract IEnumerable<string> Operate();

    private void SetWarnings(IEnumerable<string> warnings)
    {
        foreach (string warning in warnings)
        {
            AddSuccess(new CSVWarning(warning));
        }
    }
}