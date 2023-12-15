// <copyright file="OperateCSVHandlerBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using FluentResults;
using RSBingo_Framework.CSV;

internal abstract class OperateCSVHandlerBase<LineType> : RequestHandler<OperateCSVRequest>
    where LineType : CSVLine
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly IWebServices webServices;

    protected abstract string SuccessResponse { get; }
    protected string FileName { get; } = Guid.NewGuid().ToString() + ".csv";
    protected CSVData<LineType> Data { get; private set; } = null!;

    public OperateCSVHandlerBase(IWebServices webServices) : base(semaphore)
    {
        this.webServices = webServices;
    }

    protected override async Task Process(OperateCSVRequest request, CancellationToken cancellationToken)
    {
        Result result = await webServices.DownloadFile(request.Attachment.Url, FileName);
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