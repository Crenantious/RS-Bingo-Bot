// <copyright file="UserHasNoAcceptedDropEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.DataParsers;
using RSBingo_Framework.Models;

public class UserHasNoAcceptedDropEvidenceTSV : TileSelectValidator<User, UserHasNoAcceptedDropEvidenceDP>, IUserHasNoAcceptedDropEvidenceTSV
{
    public UserHasNoAcceptedDropEvidenceTSV(UserHasNoAcceptedDropEvidenceDP parser) : base(parser)
    {

    }

    protected override bool Validate(UserHasNoAcceptedDropEvidenceDP data) =>
        data.Evidence.Count() == 0;
}