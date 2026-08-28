using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using LeagueAncients.Core.Multiplayer;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Core.Content.Ancients;


public class Leona : LeagueAncientsAncientModel
{
    public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/morgana.tscn";

    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => !RunConfigSnapshot.Active.DisableLeagueAncients && act.Index == 2;

    /*
     Relics: duty
      
      <> Start each combat with 5 Dexterity. For the first 5 rounds of combat, lose 1 at the end of your turn.
      <Locket of the Iron Solari> MP Only. Everyone gets 20 block. Exhaust
      <> Gain 2 Energy on every even turn.
      
      <Heavy Hitter> While you are at or above 10 Block, gain 3 Strength. 
      <>
      <>
      
      <>
      <>
      
     */
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3];
    
    private IEnumerable<EventOption> OptionPool1 => [

    ];
    private IEnumerable<EventOption> OptionPool2 => [

    ];
    private IEnumerable<EventOption> OptionPool3 => [

    ];
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        var firstOption = Rng.NextItem(OptionPool1)!;
        var secondOption = Rng.NextItem(OptionPool2)!;
        var thirdOption = Rng.NextItem(OptionPool3)!;

        return 
        [
                    firstOption,
                    secondOption,
                    thirdOption,
        ];
    }
}