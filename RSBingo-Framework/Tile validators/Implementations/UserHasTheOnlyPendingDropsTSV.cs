// <copyright file="UserHasTheOnlyPendingDropsTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingo_Framework.TileValidators;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasTheOnlyPendingDropsTSV : TileSelectValidator<Tile, User, IUserHasTheOnlyPendingDropsDP>, IUserHasTheOnlyPendingDropsTSV
{
    public override string ErrorMessage => "Another team member has evidence pending for this tile.";

    public UserHasTheOnlyPendingDropsTSV(IUserHasTheOnlyPendingDropsDP parser) : base(parser)
    {

    }

    protected override bool Validate(IUserHasTheOnlyPendingDropsDP data) =>
        data.Evidence.Count() == 0;
}