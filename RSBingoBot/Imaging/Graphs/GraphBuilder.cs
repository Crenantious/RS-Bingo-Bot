// <copyright file="GraphBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Imaging.Graphs;

using RSBingoBot.DTO;

public class GraphBuilder
{
    protected string Title { get; }
    protected GraphAxisInfo XAxisInfo { get; }
    protected GraphAxisInfo YAxisInfo { get; }

    public GraphBuilder(string title, GraphAxisInfo xAxisInfo, GraphAxisInfo yAxisInfo) 
    {
        Title = title;
        XAxisInfo = xAxisInfo;
        YAxisInfo = yAxisInfo;
    }
}