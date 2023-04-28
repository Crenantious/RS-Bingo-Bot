// <copyright file="OperatorResults.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework_Tests.DTO;

public record class OperatorResults(Type? ExceptionType, List<Type> WarningTypes);