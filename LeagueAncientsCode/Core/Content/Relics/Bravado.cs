using LeagueAncients.Core.Content.Cards;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LeagueAncients.Core.Content.Relics;


public class Bravado : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    
    private bool LastPlayedCardWasSkill
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => [HoverTipFactory.FromCard<Gem>()];

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature)) return Task.CompletedTask;
        LastPlayedCardWasSkill = false;
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack && LastPlayedCardWasSkill)
        {
            Flash();
            var players = Owner.Creature.CombatState!.GetTeammatesOf(Owner.Creature)
                        .Where(c => c is { IsPlayer: true, IsAlive: true })
                        .Select(c => c.Player)
                        .ToList();
            for(var i = DynamicVars.Cards.IntValue; i > 0; i--)
            {
                var player = Owner.RunState.Rng.CombatTargets.NextItem(players);
                if (player is null) continue;
                Owner.Creature.CombatState.CreateCard<Gem>(player);
            }
        }
        LastPlayedCardWasSkill = cardPlay.Card.Type == CardType.Skill;
        return Task.CompletedTask;
    }
}