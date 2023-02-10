// <copyright file="CSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.CSV_reader;

using System.Net;
using Microsoft.EntityFrameworkCore;
using RSBingo_Common;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Interfaces;
using RSBingo_Framework.Models;
using SixLabors.ImageSharp;
using static RSBingo_Framework.DAL.DataFactory;
using static RSBingo_Common.General;
using RSBingo_Framework.CSV_reader.CSV_lines;

/// <summary>
/// Reads and parses a CSV file, then updates the database accordingly.
/// </summary>
public abstract class CSVReader<LineType> where LineType : CSVLine
{
    protected readonly IDataWorker DataWorker = CreateDataWorker();
    protected LineType Line = Activator.CreateInstance<LineType>();

    private int currentFileLine = 1;

    public string Parse(string filePath)
    {
        try
        {
            PreParsing();
            ParseFile(filePath);
            PostParsing();
        }
        catch (CSVReaderException e)
        {
            return GetFileExceptionMessage(e.Message);
        }
        catch (Exception e)
        {
            General.LoggingLog(e, e.Message);
            return "Internal error.";
        }

        return string.Empty;
    }

    protected virtual void PreParsing() { }
    protected virtual void PostParsing() { }
    protected abstract void LineSuccessfullyParsed();

    private void ParseFile(string fileName)
    {
        using (StreamReader reader = new(fileName))
        {
            currentFileLine = 1;

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine()!;
                if (line == null) { throw new CSVReaderException("Unable to read"); }

                string[] values = line.Split(',');

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i][0] is ' ') { values[i] = values[i][1..]; }
                }

                Line.Parse(values);
                LineSuccessfullyParsed();
                currentFileLine++;
            }
        }
    }

    private string GetFileExceptionMessage(string prefix) =>
        $"{prefix} on line {currentFileLine}.";
}