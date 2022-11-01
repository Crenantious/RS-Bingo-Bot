// <copyright file="TileRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static RSBingo_Framework.Repository.EvidenceRepository;

    public static class TileRecord
    {
        public static bool IsVerified(this Tile tile) =>
            tile.Verfied == 1;

        public static bool IsNotVerified(this Tile tile) =>
            tile.Verfied != 1;
    }
}