// <copyright file="CSVOperator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;
using RSBingo_Framework.Interfaces;
using System.Text;

/// <summary>
/// Performs operations based on the contents of a CSV file.
/// </summary>
public abstract class CSVOperator<LineType> where LineType : CSVLine
{
    protected virtual string WarningMessagesPrefix => "Process was successful but had the following warnings: ";

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

    /// <summary>
    /// Gets messages compiled with warnings that occurred while operating on the <see cref="CSVData{LineType}"/>.
    /// Each message can contain multiple warnings, up to the amount allowed by <paramref name="maxMessageChars"/>.
    /// </summary>
    /// <param name="maxMessageChars">The maximum amount of characters each message can contain.
    /// If adding a warning to the message would exceed this value, warnings will stop being added and a new message will begin.</param>
    /// <returns>The compiled warning messages.</returns>
    public IEnumerable<string> GetCompiledWarningMessages(int maxMessageChars)
    {
        List<string> warningMessages = new();
        if (Warnings.Count == 0) { return warningMessages; }

        IEnumerable<string> remainingWarnings = new List<string>() { WarningMessagesPrefix };
        remainingWarnings.Concat(Warnings);

        while (remainingWarnings.Any())
        {
            (string warningMessage, remainingWarnings) = GetFirstWarningMessage(remainingWarnings, maxMessageChars);

            // This means maxMessageChars is less than the length of the first warning thus none can be added to the message.
            // Not breaking would cause an infinite loop.
            if (string.IsNullOrEmpty(warningMessage)) { break; }

            warningMessages.Add(warningMessage);
        }

        return warningMessages;
    }

    public IEnumerable<string> GetRawWarnings() =>
        new List<string>(Warnings);

    /// <summary>
    /// Adds a warning of the form $"{prefix} on line {line.LineNumber}".
    /// </summary>
    /// <param name="prefix">The prefix of the message.</param>
    /// <param name="line">The line the warning occurred on.</param>
    protected void AddWarning(string prefix, LineType line) =>
        Warnings.Add($"{prefix} on line {line.LineNumber}");

    private void OperateOnLines(CSVData<LineType> data)
    {
        foreach (LineType line in data.Lines)
        {
            OperateOnLine(line);
        }
    }

    private (string message, IEnumerable<string> reminingWarnings) GetFirstWarningMessage(IEnumerable<string> warnings, int maxMessageSize)
    {
        StringBuilder warningMessage = new StringBuilder();
        int warningIndex = 0;

        foreach (string warning in warnings)
        {
            if (warningMessage.Length + Environment.NewLine.Length + warning.Length > maxMessageSize) { break; }
            warningIndex++;
            warningMessage.AppendLine(warning);
        }
        return (warningMessage.ToString(), warnings.Skip(warningIndex));
    }
}