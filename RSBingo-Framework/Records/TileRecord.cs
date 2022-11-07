// <copyright file="TileRecord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.Records
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RSBingo_Common;
    using RSBingo_Framework.Interfaces;
    using RSBingo_Framework.Models;
    using static RSBingo_Framework.Repository.EvidenceRepository;

    public static class TileRecord
    {
        #region enums & lookups

        private static readonly EnumDict<VerifiedStatus> VerifiedLookup = new EnumDict<VerifiedStatus>(VerifiedStatus.No)
         .Add(VerifiedStatus.Yes, 1);

        public enum VerifiedStatus
        {
            No,
            Yes
        }

        #endregion

        public static bool IsVerified(this Tile tile) =>
            tile.Verified == 1;

        public static bool IsNotVerified(this Tile tile) =>
            tile.Verified != 1;
    }
}