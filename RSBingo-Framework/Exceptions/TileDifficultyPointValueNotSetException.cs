// <copyright file="BoardImageException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TileDifficultyPointValueNotSetException : RSBingoException
{
    public TileDifficultyPointValueNotSetException(string? messsage) : base(messsage) { }
}