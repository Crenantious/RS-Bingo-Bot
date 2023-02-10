// <copyright file="RemoveTasksCSVReader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using RSBingo_Framework.CSV_reader.CSV_lines;

namespace RSBingo_Framework.CSV_reader;

/// <inheritdoc/>
public class RemoveTasksCSVReader : CSVReader<RemoveTaskCSVLine>
{
    protected override void LineSuccessfullyParsed()
    {
        DataWorker.BingoTasks.DeleteMany(
            DataWorker.BingoTasks.GetByNameAndDifficulty(Line.TaskName, Line.TaskDifficulty)
            .Take(Line.AmountOfTasks));
    }

    protected override void PostParsing() =>
        DataWorker.SaveChanges();
}