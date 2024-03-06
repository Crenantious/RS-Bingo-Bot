// <copyright file="EvidenceRejectionReactionValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RSBingoBot.Requests;

using DiscordLibrary.Requests.Extensions;
using DiscordLibrary.Requests.Validation;

internal class EvidenceRejectionReactionValidator : Validator<EvidenceRejectionReactionRequest>
{
    public EvidenceRejectionReactionValidator()
    {
        // TODO: JR - validate user's role (is a host/admin).
        EmojiMatches(r => (r.GetEmoji(), r.VerificationEmoji));
    }
}