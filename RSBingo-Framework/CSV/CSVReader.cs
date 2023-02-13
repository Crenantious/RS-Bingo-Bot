// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV;

using System.Net;
using Microsoft.EntityFrameworkCore;
using RSBingo_Common;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Common.General;
using RSBingo_Framework.CSV.Lines;
using System.Runtime.CompilerServices;

/// <summary>
/// Reads and parses a CSV file.
/// </summary>
public static class CSVReader
{
    private static int currentLineNumber;

    /// <summary>
    /// Opens and parses the CSV file at <paramref name="filePath"/>.
    /// For each line, if it is parsed successfully, an operation is ran on it depending on the <see cref="CSVReader"/> type.
    /// </summary>
    /// <param name="filePath">The path to the CSV file to be parsed.</param>
    /// <returns>An error message, if not <see langword="null"/>.</returns>
    /// <exception cref="CSVReaderException">Thrown if unable to read a line, or the data is in an unexpected format.</exception>
    /// <exception cref="Exception">Generic exception thrown when an unexpected error occurs i.e. does not have read permission of the file.</exception>
    public static CSVData<LineType> Parse<LineType>(string filePath) where LineType : CSVLine
    {
        try
        {
            StreamReader reader = new(filePath);
            return ParseFile<LineType>(reader);
        }
        catch (CSVReaderException e)
        {
            // Wrap message, but keep original exception to see stack trace,
            throw new CSVReaderException($"{e.Message} on line {currentLineNumber}.", e);
        }
        catch
        {            
            // Re-throw to keep stack trace.
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

            lines.Add((LineType)Activator.CreateInstance(typeof(LineType), currentLineNumber, values)!);
        }

        return new CSVData<LineType>(lines);
    }

    private static string GetNextLine(StreamReader reader)
    {
        string? line = reader.ReadLine();
        if (line == null) { throw new CSVReaderException("Unable to read"); }
        return line;
    }

    private static IEnumerable<string> GetLineValues(string line) => line.Split(',').Select(s => s.Trim());
}