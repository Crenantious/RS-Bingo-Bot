// <copyright file="CSVHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.DiscordServices;
using DiscordLibrary.Requests;
using FluentResults;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.CSV;

internal abstract class CSVHandler<TRequest, LineType> : RequestHandler<TRequest>
    where TRequest : ICSVRequest
    where LineType : CSVLine
{
    private readonly IWebServices webServices;

    protected string FileName { get; } = Guid.NewGuid().ToString() + ".csv";
    protected CSVData<LineType> Data { get; private set; } = null!;
    protected IDataWorker DataWorker { get; } = DataFactory.CreateDataWorker();

    public CSVHandler()
    {
        this.webServices = (IWebServices)General.DI.GetService(typeof(IWebServices))!;
    }

    protected override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        Result result = await webServices.DownloadFile(request.Attachment.Url, request.FileName);
        if (result.IsFailed)
        {
            AddErrors(result.Errors);
            return;
        }

        Operate(ParseFile());
    }

    private protected CSVData<LineType> ParseFile()
    {
        CSVReader reader = new();
        return reader.Parse<LineType>(FileName);
    }

    private protected abstract void Operate(CSVData<LineType> data);
}