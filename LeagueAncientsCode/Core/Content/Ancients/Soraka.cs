using BaseLib.Utils;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LeagueAncients.Core.Content.Ancients;

// TODO: Not an Ancient. Pantheon would replace her as the Neow replacement. See what Offers can be taken over by Panth
public class Soraka : LeagueAncientsAncientModel
{
    public override string? CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string? CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string? CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";

    //public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/soraka.tscn";
    public override string? CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/soraka.tscn";
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => act.Index == 0;
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3];

    private IEnumerable<EventOption> OptionPool1 => [
                RelicOption<SmallCapsule>(),
                RelicOption<LargeCapsule>(),
                RelicOption<GoldenPearl>(),
                RelicOption<CursedPearl>(),
                RelicOption<StoneHumidifier>(),
    ];
    private IEnumerable<EventOption> OptionPool2 => [
                RelicOption<ArcaneScroll>(),
                RelicOption<MassiveScroll>(),
                RelicOption<ScrollBoxes>(),
                RelicOption<WingedBoots>(),
                RelicOption<PhialHolster>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<TimeCapsule>(),
                RelicOption<RejuvenationBead>(),
                RelicOption<SorakasCompassionRelic>(),
    ];
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        return 
        [
                    Rng.NextItem(OptionPool1)!,
                    Rng.NextItem(OptionPool2)!,
                    Rng.NextItem(OptionPool3)!,
        ];
    }
}