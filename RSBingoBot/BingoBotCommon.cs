using DSharpPlus;
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
        public static DiscordEmoji EvidencePendingReviewEmoji;
        public static DiscordEmoji EvidenceVerifiedEmoji;
        public static DiscordEmoji EvidenceRejectedEmoji;

        static BingoBotCommon()
        {
            DiscordClient client = (DiscordClient)General.DI.GetService(typeof(DiscordClient));
            EvidencePendingReviewEmoji = DiscordEmoji.FromUnicode(client, "⌛");
            EvidenceVerifiedEmoji = DiscordEmoji.FromUnicode(client, "✅");
            EvidenceRejectedEmoji = DiscordEmoji.FromUnicode(client, "❌");
        }

        public static DiscordEmoji? GetEvidenceStatusEmoji(Evidence evidence) =>
            evidence.Status switch
            {
                (sbyte)EvidenceStatus.PendingReview => EvidencePendingReviewEmoji,
                (sbyte)EvidenceStatus.Verified => EvidenceVerifiedEmoji,
                (sbyte)EvidenceStatus.Rejected => EvidenceRejectedEmoji,
                _ => null
            };
    }
}
