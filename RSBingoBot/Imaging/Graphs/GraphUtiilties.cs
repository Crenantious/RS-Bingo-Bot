// <copyright file="GraphUtiilties.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;

public static class GraphUtiilties
{
    // TODO: JR - fix for use with generics
    //private const string LabelCountOutOfRange = "{0} must be greater than zero. Was {1}.";

    //public static IEnumerable<string> GetAxisLabelsFromData<TData>(IEnumerable<TData> categories, int axisLabelCount)
    //    where TData : IComparable<TData>
    //{
    //    if (axisLabelCount < 1)
    //    {
    //        throw new ArgumentOutOfRangeException(LabelCountOutOfRange.FormatConst(nameof(axisLabelCount), axisLabelCount));
    //    }

    //    (TData min, TData max) = GetAxiesBoudaries(categories);
    //    float
    //    string[] labels = new string[axisLabelCount];
    //    float step = axisLabelCount == 1 ? 0 : (max - min) / (axisLabelCount - 1);

    //    for (int i = 0; i < axisLabelCount; i++)
    //    {
    //        labels[i] = (min + (step * i)).ToString();
    //    }

    //    return labels;
    //}

    //private static (TData min, TData max) GetAxiesBoudaries<TData>(IEnumerable<TData> dataSet)
    //    where TData : IComparable<TData>
    //{
    //    TData min = default;
    //    TData max = default;

    //    foreach (TData data in dataSet)
    //    {
    //        if (data.CompareTo(min) < 0) { min = data; }
    //        if (data.CompareTo(max) > 0) { max = data; }
    //    }
    //    return (min, max);
    //}
}