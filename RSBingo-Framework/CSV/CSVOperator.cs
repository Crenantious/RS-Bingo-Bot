// <copyright file="CSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using System.Text;

/// <summary>
/// Performs operations based on the contents of a CSV file.
/// </summary>
public abstract class CSVOperator<LineType> where LineType : CSVLine
{
    protected virtual string ErrorMessagePrefix => "The following errors occurred: ";

    protected string? ErrorMessage = null;

    private StringBuilder? errorMessageStringBuilder = null;

    /// <summary>
    /// Performs operations based on the data provided.
    /// </summary>
    /// <exception cref="CSVOperatorException"/>
    public void Operate(CSVData<LineType> data)
    {
        errorMessageStringBuilder = null;
        ErrorMessage = null;

        OnPreOperating();
        OperateOnLines(data);
        OnPostOperating();
        FinaliseErrorMessage();

        if (ErrorMessage is not null) { throw new CSVOperatorException(ErrorMessage); }
    }

    protected virtual void OnPreOperating() { }
    protected abstract void OperateOnLine(LineType line);
    protected virtual void OnPostOperating() { }

    protected void AddLineToErrorMessage(LineType line, string prefix)
    {
        if (errorMessageStringBuilder is null)
        {
            errorMessageStringBuilder = new StringBuilder(ErrorMessagePrefix);
        }

        errorMessageStringBuilder.AppendLine($"{prefix} on line {line.LineNumber}");
    }

    private void OperateOnLines(CSVData<LineType> data)
    {
        foreach (LineType line in data.Lines)
        {
            OperateOnLine(line);
        }
    }

    private void FinaliseErrorMessage()
    {
        if (errorMessageStringBuilder != null)
        {
            //errorMessageStringBuilder.Append(".");
            ErrorMessage = errorMessageStringBuilder.ToString();
        }
    }
}