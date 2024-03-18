// <copyright file="UserHasTheOnlyPendingDropsTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasTheOnlyPendingDropsTSV : TileSelectValidator<Tile, User, UserHasTheOnlyPendingDropsDP>, IUserHasTheOnlyPendingDropsTSV
{
    public UserHasTheOnlyPendingDropsTSV(UserHasTheOnlyPendingDropsDP parser) : base(parser)
    {

    }

    protected override bool Validate(UserHasTheOnlyPendingDropsDP data) =>
        data.Evidence.Count() == 0;
}