using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Relics;


/// <summary>
/// Gain Vigor after playing Skills
/// </summary>
public class MoonSilver() : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VigorPower>(2)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Player != Owner && cardPlay.Card.Type is not CardType.Skill) return;
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars[nameof(VigorPower)].BaseValue, Owner.Creature, null);
    }
    
    // Leave this at 2 vigor per skill for now. That already seems decent.
    // If I do end up wanting Vigor multiplicative scaling I likely need to create an Interface and iterate over it in vigor power
}