using DSharpPlus.Entities;
using RSBingo_Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RSBingo_Framework.Records.EvidenceRecord;

namespace RSBingoBot
{
    internal class BingoBotCommon
    {
        public static DiscordComponentEmoji EvidencePendingReviewEmoji = new DiscordComponentEmoji("⌛");
        public static DiscordComponentEmoji EvidenceVerifiedEmoji = new DiscordComponentEmoji("✅");
        public static DiscordComponentEmoji EvidenceRejectedReviewEmoji = new DiscordComponentEmoji("❌");

        public static DiscordComponentEmoji GetEvidenceStatusEmoji(Evidence evidence) =>
            evidence.Status switch
            {
                (sbyte)EvidenceStatus.PendingReview => EvidencePendingReviewEmoji,
                (sbyte)EvidenceStatus.Verified => EvidencePendingReviewEmoji,
                (sbyte)EvidenceStatus.Rejected => EvidencePendingReviewEmoji,
                _ => new DiscordComponentEmoji()
            };
    }
}
