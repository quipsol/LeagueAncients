using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Core.Content.Enchantments;

public class Shareholder : CustomEnchantmentModel
{
    protected override string CustomIconPath => "res://LeagueAncients/images/placeholder/100_100/blue.png";
    public override bool HasExtraCardText => false;
    public override bool ShowAmount => false;
    public override bool CanEnchant(CardModel card) => card.Type is CardType.Attack or CardType.Skill && !card.Keywords.Contains(CardKeyword.Exhaust);
    protected override void OnEnchant() { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources, CardLocation cardLocation)
    {
        if (card.Enchantment is not Shareholder) return cardLocation;
        var players = card.Owner.Creature.CombatState!.GetTeammatesOf(card.Owner.Creature)
                    .Where(c => c is { IsPlayer: true, IsAlive: true } && c.Player != card.Owner)
                    .ToList();
        if (players.Count == 0) return cardLocation;
        cardLocation.player = card.Owner.RunState.Rng.CombatTargets.NextItem(players)!.Player!;
        return cardLocation;
    }

    public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation cardLocation)
    {
        if(cardLocation.player != card.Owner) 
            card.EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }
}