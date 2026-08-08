using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace LeagueAncients.Core.Content.Cards;


[Pool(typeof(EventCardPool))]
public class AchieveDivinityKayle() : LeagueAncientsCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate, CardKeyword.Exhaust];

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var card in Owner.PlayerCombatState?.AllCards.Where(c => c.Type is CardType.Attack || (IsUpgraded && c.Type is CardType.Power)) ?? [])
            if (card.IsUpgradable && card != this) CardCmd.Upgrade(card);
        return Task.CompletedTask;
    }
}