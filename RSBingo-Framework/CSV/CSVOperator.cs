// <copyright file="CSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Operators.Warnings;
using RSBingo_Framework.Interfaces;
using System.Text;

/// <summary>
/// Performs operations based on the contents of a CSV file.
/// </summary>
public abstract class CSVOperator<LineType>
    where LineType : CSVLine
{
    private const string WarningMessagesPrefix = "Process was successful but had the following warnings: ";

    private protected readonly List<Warning> warnings = new();
    private protected readonly List<string> warningMessages = new();
    private protected IDataWorker DataWorker;

    public CSVOperator(IDataWorker dataWorker)
    {
        DataWorker = dataWorker;
    }

    public IEnumerable<Warning> GetRawWarnings() =>
        new List<Warning>(warnings);

    public IEnumerable<string> GetWarningMessages() =>
        warningMessages.ToList();

    /// <summary>
    /// Performs operations based on the data provided.
    /// </summary>
    /// <exception cref="CSVOperatorException"></exception>
    public void Operate(CSVData<LineType> data)
    {
        warnings.Clear();
        warningMessages.Clear();

        OnPreOperating();
        OperateOnLines(data);
        OnPostOperating();
    }

    /// <summary>
    /// Called before <see cref="OperateOnLine({LineType})"/>.
    /// Used to setup the environment and for any pre-processing logic before for operating.
    /// </summary>
    protected virtual void OnPreOperating() { }
    
    /// <summary>
    /// Perform logic based on the data in <paramref name="line"/>.
    /// </summary>
    protected abstract void OperateOnLine(LineType line);

    /// <summary>
    /// Called after <see cref="OperateOnLine({LineType})"/>.
    /// Used to cleanup the environment and for any post-processing logic after operating.
    /// </summary>
    protected virtual void OnPostOperating() { }

    /// <summary>
    /// Adds a warning and its associated message.
    /// </summary>
    protected void AddWarning<WarningType>(WarningType warning) where WarningType : Warning
    {
        warnings.Add(warning);
        warningMessages.Add($"{warning.Message} on line {warning.LineNumber}.");
    }

    private void OperateOnLines(CSVData<LineType> data)
    {
        foreach (LineType line in data.Lines)
        {
            OperateOnLine(line);
        }
    }
}