using LeagueAncients.Core.AncientPotions;
using MegaCrit.Sts2.Core.Entities.Potions;

namespace LeagueAncients.Extensions;

public static class PotionExtensions
{
    extension(PotionRarity pr)
    {
        public static PotionRarity Ancient => CustomPotionRarity.Ancient;
    }
}