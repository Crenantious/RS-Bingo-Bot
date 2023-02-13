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
    /// <exception cref="CSVReaderException"/>
    public static CSVData<LineType> Parse<LineType>(string filePath) where LineType : CSVLine
    {
        try
        {
            using (StreamReader reader = new(filePath))
            {
                return ParseFile<LineType>(reader);
            }
        }
        catch (CSVReaderException e)
        {
            throw new CSVReaderException($"{e.Message} on line {currentLineNumber}.");
        }
        catch (Exception e)
        {
            General.LoggingLog(e, e.Message);
            throw e;
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
            string[] values = GetLineValues(line);

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

    private static string[] GetLineValues(string line)
    {
        string[] values = line.Split(',');

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i][0] is ' ') { values[i] = values[i][1..]; }
        }

        return values;
    }
}