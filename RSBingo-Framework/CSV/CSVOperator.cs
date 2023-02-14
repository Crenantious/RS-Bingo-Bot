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

    private protected List<string> Warnings = new();

    /// <summary>
    /// Performs operations based on the data provided.
    /// </summary>
    /// <exception cref="CSVOperatorException"></exception>
    public void Operate(CSVData<LineType> data)
    {
        Warnings = new();

        OnPreOperating();
        OperateOnLines(data);
        OnPostOperating();
    }

    protected virtual void OnPreOperating() { }
    protected abstract void OperateOnLine(LineType line);
    protected virtual void OnPostOperating() { }

    protected void AddWarning(LineType line, string prefix)
    {
        Warnings.Add($"{prefix} on line {line.LineNumber}");
    }

    private void OperateOnLines(CSVData<LineType> data)
    {
        foreach (LineType line in data.Lines)
        {
            OperateOnLine(line);
        }
    }

    public string GetWarningMessage(int maxMessageSize)
    {
        if (Warnings.Count == 0) { return string.Empty; } 
        StringBuilder warningMessage = new StringBuilder($"Process was successful but had {Warnings.Count} warnings. Here are the first warnings: ");
        foreach (string warning in Warnings)
        {
            if (warning.Length + warningMessage.Length + Environment.NewLine.Length > maxMessageSize) { break; }
            warningMessage.AppendLine(warning);
        }

        return warningMessage.ToString();
    }
}