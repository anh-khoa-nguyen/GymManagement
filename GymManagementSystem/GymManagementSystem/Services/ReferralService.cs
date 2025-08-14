// Trong file Services/ReferralService.cs
using System.Collections.Generic;
using System.Linq;

public class ReferralService
{
    public static Dictionary<int, string> RewardTiers { get; } = new Dictionary<int, string>
    {
        { 1, "REFERRAL_5" },   // Mã KM cho mốc 5 người
        { 10, "REFERRAL_10" }, // Mã KM cho mốc 10 người
        { 15, "REFERRAL_15" }  // Mã KM cho mốc 15 người
    };

    public static int GetNextRewardTier(int currentReferralCount)
    {
        foreach (var tier in RewardTiers.Keys.OrderBy(k => k))
        {
            if (currentReferralCount < tier)
            {
                return tier;
            }
        }
        return 0;
    }
}