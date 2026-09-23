using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LeagueAncients.Core.Content.Relics;


public class RingOfFriendship : LeagueAncientsRelicModel
{
    private const string BASE_GOLD_INCREASE = "BaseGoldIncrease";
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new (BASE_GOLD_INCREASE, 1.10m)];

    public override decimal ModifyGoldGained(Player player, decimal amount)
    {
        if (player != Owner) return amount;
        return amount * ((DynamicVars[BASE_GOLD_INCREASE].BaseValue - 1) * Owner.RunState.Players.SelectMany(p => p.Relics).Count(r => r is RingOfFriendship) + 1);
    }

    public override Task AfterModifyingGoldGained(Player player, decimal amount)
    {
        Flash();
        return Task.CompletedTask;
    }
    
}