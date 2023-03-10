// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;
using System.Runtime.ExceptionServices;

/// <summary>
/// Reads and parses a CSV file.
/// </summary>
public static class CSVReader
{
    private const string CSVFileExtension = ".csv";

    private static int currentLineNumber;

    /// <summary>
    /// Opens and parses the CSV file at <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>The data parsed from the file.</returns>
    /// <exception cref="FormatException">The file at <paramref name="filePath"/> is not a .csv file.</exception>
    /// <exception cref="CannotReadLineCSVReaderException">The file contains a line that is unreadable.</exception>
    /// <exception cref="IncorrectNumberOfCSVValuesException">The file contains a line that does not have the minimum number of required values.</exception>
    /// <exception cref="InvalidCSVValueTypeException">The file contains a line that has a value that cannot be cast to the required type.</exception>
    /// <exception cref="CSVReaderException">Is what is actually thrown instead of the previous exceptions and is it their parent.</exception>
    /// <exception cref="Exception">An unexpected error occurred, i.e. does not have permission to read the file.</exception>
    public static CSVData<LineType> Parse<LineType>(string filePath) where LineType : CSVLine
    {
        try
        {
            if (filePath.EndsWith(CSVFileExtension) is false)
            {
                throw new InvalidFileTypeException($"The file at {filePath} must be of type .csv.");
            }

            using (StreamReader reader = new(filePath))
            return ParseFile<LineType>(reader);
        }
        catch (CSVReaderException e)
        {
            // Wrap message, but keep original exception to see stack trace,
            throw (CSVReaderException)Activator.CreateInstance(e.GetType(), $"{e.Message}, on line {currentLineNumber}.", e);
        }
        catch (Exception e)
        {
            //TODO: figure out how stack traces work and what the best solution is here;
            // Re-throw to keep stack trace.
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    private static CSVData<LineType> ParseFile<LineType>(StreamReader streamReader) where LineType : CSVLine
    {
        List<LineType> lines = new();
        currentLineNumber = 0;

        while (streamReader.EndOfStream is false)
        {
            currentLineNumber++;
            string line = GetNextLine(streamReader);
            IEnumerable<string> values = GetLineValues(line);
            AddLine(lines, values);
        }

        return new CSVData<LineType>(lines);
    }

    private static string GetNextLine(StreamReader reader)
    {
        string? line = reader.ReadLine();
        if (line == null) { throw new CannotReadCSVLineException("Unable to read"); }
        return line;
    }

    private static IEnumerable<string> GetLineValues(string line) =>
        line.Split(',').Select(s => s.Trim());

    private static void AddLine<LineType>(List<LineType> lines, IEnumerable<string> values) where LineType : CSVLine
    {
        try
        {
            lines.Add((LineType)Activator.CreateInstance(typeof(LineType), currentLineNumber, values.ToArray())!);
        }
        catch (System.Reflection.TargetInvocationException e)
        {
            throw e.InnerException;
        }
    }
}