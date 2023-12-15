// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using RSBingo_Framework.Exceptions.CSV;
using System.Runtime.ExceptionServices;

/// <summary>
/// Reads and parses a CSV file.
/// </summary>
public class CSVReader
{
    private const string CSVFileExtension = ".csv";

    private int lineNumber = 0;

    /// <summary>
    /// Opens and parses the CSV file at <paramref name="filePath"/>.
    /// </summary>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>The data parsed from the file.</returns>
    /// <exception cref="InvalidFileTypeException">The file at <paramref name="filePath"/> is not a .csv file.</exception>
    /// <exception cref="IncorrectNumberOfCSVValuesException">The file contains a line that does not have the correct number of required values.</exception>
    /// <exception cref="InvalidCSVValueTypeException">The file contains a line that has a value that cannot be converted to the required type.</exception>
    /// <exception cref="CSVReaderException">Is what is actually thrown instead of the previous exceptions as is it their parent.</exception>
    /// <exception cref="Exception">An unexpected error occurred, i.e. does not have permission to read the file.</exception>
    public CSVData<LineType> Parse<LineType>(string filePath) where LineType : CSVLine
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
            throw (CSVReaderException)Activator.CreateInstance(e.GetType(), $"{e.Message}, on line {lineNumber}.", e)!;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static IEnumerable<string> GetLineValues(string line) =>
        line.Split(',').Select(s => s.Trim());

    private CSVData<LineType> ParseFile<LineType>(StreamReader streamReader) where LineType : CSVLine
    {
        List<LineType> lines = new();
        lineNumber = 0;

        while (streamReader.EndOfStream is false)
        {
            lineNumber++;

            string? line = streamReader.ReadLine();
            if (line is null) { break; } // TODO: JCH - What happens if a file has multiple blank lines? Write a UT to confirm this works as expected.

            IEnumerable<string> values = GetLineValues(line);
            AddLine(lines, values);
        }

        return new CSVData<LineType>(lines);
    }

    private void AddLine<LineType>(List<LineType> lines, IEnumerable<string> values) where LineType : CSVLine
    {
        try
        {
            lines.Add((LineType)Activator.CreateInstance(typeof(LineType), lineNumber, values.ToArray())!);
        }
        catch (System.Reflection.TargetInvocationException e)
        {
            throw e.InnerException!;
        }
    }
}