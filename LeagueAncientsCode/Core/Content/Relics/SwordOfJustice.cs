using BaseLib.Utils;
using LeagueAncients.Core.Content.Powers;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// After you play an attack, gain temporary Strength
/// </summary>
[Pool(typeof(EventRelicPool))]
public class SwordOfJustice : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SwordOfJusticeStrengthPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type is not CardType.Attack)
            return;
        await PowerCmd.Apply<SwordOfJusticeStrengthPower>(context, Owner.Creature, DynamicVars[nameof(SwordOfJusticeStrengthPower)].BaseValue, Owner.Creature, null);
    }
}