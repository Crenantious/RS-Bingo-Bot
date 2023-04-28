// <copyright file="ReaderResults.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DTO;

using RSBingo_Framework.CSV;

public record class ReaderResults<LineType>(CSVData<LineType>? data, Type? exceptionType) where LineType : CSVLine;