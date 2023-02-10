// <copyright file="AddTaskRestrictionsCSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.CSV_reader.CSV_lines;
using RSBingo_Framework.Exceptions;
using RSBingo_Framework.Models;

namespace RSBingo_Framework.CSV_reader;

/// <inheritdoc/>
public class AddTaskRestrictionsCSVReader : CSVReader<AddTaskRestrictionCSVLine>
{
    private static readonly Dictionary<string, int> RestrictionNameToId = new();
    private static readonly HashSet<int> TaskIds = new();

    protected override void LineSuccessfullyParsed()
    {
        if (RestrictionNameToId.ContainsKey(Line.RestrictionName))
        {
            throw new CSVReaderException("Duplicate restriction name found");
        }

        Restriction restriction = DataWorker.Restrictions.Create(Line.RestrictionDescription);
        RestrictionNameToId.Add(Line.RestrictionName, restriction.RowId);
    }

    protected override void PostParsing() =>
        DataWorker.SaveChanges();
}