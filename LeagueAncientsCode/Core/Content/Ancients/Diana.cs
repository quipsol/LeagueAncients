using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using LeagueAncients.Core.Multiplayer;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Core.Content.Ancients;


public class Diana : LeagueAncientsAncientModel
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
     Relics: revelation
      
      <> Every third attack (in a turn?) deals 10 extra damage
      <> The third time you take damage each combat, gain 20 Block and deal 20 damage to ALL enemies
      <> Gain 2 Energy on every Odd turn.
      
      <Tear of the goddess> conserve mana into next combat
      <Dusk and Dawn> The first Power you play each combat is played twice
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