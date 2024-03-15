// <copyright file="SubmitVerificationEvidenceTSV.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using RSBingo_Framework.Models;

public class SubmitVerificationEvidenceTSV : ISubmitVerificationEvidenceTSV
{
    public bool Validate(User user) =>
        user.Evidence
            .GetVerificationEvidence()
            .GetAcceptedEvidence()
            .Count() == 0;
}