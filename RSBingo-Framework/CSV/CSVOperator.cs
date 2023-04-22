// <copyright file="CSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.CSV.Operators.Warnings;
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

    /// <summary>
    /// Gets messages compiled with warnings that occurred while operating on the <see cref="CSVData{LineType}"/>.
    /// Each message can contain multiple warnings, up to the amount allowed by <paramref name="maxMessageChars"/>.
    /// </summary>
    /// <param name="maxMessageChars">The maximum amount of characters each message can contain.
    /// If adding a warning to a message would exceed this value, warnings will stop being added to that message and a new one will begin.</param>
    /// <returns>The compiled warning messages.</returns>
    public IEnumerable<string> GetCompiledWarningMessages(int maxMessageChars)
    {
        List<string> compiledMessages = new();
        if (warnings.Any() is false) { return compiledMessages; }

        IEnumerable<string> remainingWarnings = new List<string>() { WarningMessagesPrefix };
        remainingWarnings.Concat(warningMessages);

        while (remainingWarnings.Any())
        {
            (string compiledMessage, remainingWarnings) = GetFirstCompiledWarningMessage(remainingWarnings, maxMessageChars);

            // This means maxMessageChars is less than the length of the first warning thus none can be added to the message.
            // Not breaking would cause an infinite loop.
            if (string.IsNullOrEmpty(compiledMessage)) { break; }

            compiledMessages.Add(compiledMessage);
        }

        return compiledMessages;
    }

    public IEnumerable<Warning> GetRawWarnings() =>
        new List<Warning>(warnings);

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
        warningMessages.Add($"{warning.Message} for value {warning.ValueIndex} on line {warning.LineNumber}");
    }

    private void OperateOnLines(CSVData<LineType> data)
    {
        foreach (LineType line in data.Lines)
        {
            OperateOnLine(line);
        }
    }

    private (string message, IEnumerable<string> reminingWarnings) GetFirstCompiledWarningMessage(IEnumerable<string> warnings, int maxMessageSize)
    {
        StringBuilder warningMessage = new StringBuilder();
        int warningIndex = -1;

        foreach (string warning in warnings)
        {
            if (warningMessage.Length + Environment.NewLine.Length + warning.Length > maxMessageSize) { break; }
            warningIndex++;
            warningMessage.AppendLine(warning);
        }
        return (warningMessage.ToString(), warnings.Skip(warningIndex));
    }
}