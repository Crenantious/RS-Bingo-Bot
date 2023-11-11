// <copyright file="OperateCSVValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests.Validation;

using FluentValidation;
using RSBingo_Framework.DAL;
using RSBingo_Framework.Interfaces;
using RSBingoBot.Requests;

internal class OperateCSVValidator : AbstractValidator<OperateCSVRequest>
{
    private IDataWorker dataWorker = DataFactory.CreateDataWorker();

    public OperateCSVValidator()
    {
        this.ValidateIsCSVFile();
    }
}