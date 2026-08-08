using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LeagueAncients.Core.Content.Ancients;

public class Morgana : LeagueAncientsAncientModel
{
    public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/morgana.tscn";

    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => !Config.DisableLeagueAncients && act.Index == 1;

    /*
     Relics:
      - <> At the start of your turn if you did not attack any enemies in your previous turn, gain 1 Energy.
      - <Black Shield> At the start of combat gain 1 Artifact.
      - <Cursed Feather> Lose 20 health and gain 45 max hp.
      - <> Enchant a power with [] -> Reduce its cost by 1, when played create a copy (not exact copy) in your draw pile
      - <> 3(2) Energy. Stun the enemy at the start of your next turn.
      - <Essence of an Angel> Apotheosis but only for skills (and powers if upgraded)
      - <Nadir> While you are at or below 30% health, gain 3 Strength and Dexterity.
      
      - <Desperate Bargain> Upon reaching the boss, obtain the other two offered relics (how to handle giving relics outside of rooms!?)
      - <Soul Shackles> Obtain the other two offered relics, get cursed with Shackles -> Unplayable, Eternal, when drawn lose 1 Energy.
     */
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3];
    
    private IEnumerable<EventOption> OptionPool1 => [
                RelicOption<EssenceOfAnAngelMorgana>(),
                RelicOption<Nadir>()
    ];
    private IEnumerable<EventOption> OptionPool2 => [
                RelicOption<BlackShield>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<SoulShacklesRelic>(),
    ];
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        var firstOption = Rng.NextItem(OptionPool1)!;
        var secondOption = Rng.NextItem(OptionPool2)!;
        var thirdOption = Rng.NextItem(OptionPool3)!;

        if (thirdOption.Relic is SoulShacklesRelic soulShacklesRelic)
        {
            soulShacklesRelic.SetRewardRelics([firstOption.Relic, secondOption.Relic]);
        }
        
        return 
        [
                    firstOption,
                    secondOption,
                    thirdOption,
        ];
    }
}