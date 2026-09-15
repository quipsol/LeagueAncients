using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LeagueAncients.Core.Content.Relics;


public class Bravado : LeagueAncientsRelicModel
{
    private const string BASE_GOLD_INCREASE = "BaseGoldIncrease";
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    private bool Triggered
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }
    private bool LastPlayedCardWasStrike
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => [HoverTipFactory.Static(StaticHoverTip.Energy)];

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature)) return Task.CompletedTask;
        Triggered = false;
        LastPlayedCardWasStrike = false;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner || Triggered) return;
        if (!(cardPlay.Card.Rarity == CardRarity.Basic && cardPlay.Card.Tags.Contains(CardTag.Strike)))
        {
            LastPlayedCardWasStrike = false;
            return;
        }

        if (LastPlayedCardWasStrike)
        {
            Triggered = true;
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }
        else
            LastPlayedCardWasStrike = true;
    }
}