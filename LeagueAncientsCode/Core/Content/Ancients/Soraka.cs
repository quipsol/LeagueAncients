using BaseLib.Utils;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LeagueAncients.Core.Content.Ancients;

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

   // public override bool IsValidForAct(ActModel act) => SlayRuneterraConfig.IsEnabled;

    
    // protected override OptionPools MakeOptionPools => new(
    //             MakePool(
    //                         AncientOption<SmallCapsule>(),
    //                         AncientOption<LargeCapsule>(),
    //                         AncientOption<GoldenPearl>(weight: 20),
    //                         AncientOption<CursedPearl>(weight: 20),
    //                         AncientOption<StoneHumidifier>(weight: 10)
    //             ),
    //             MakePool(
    //                         AncientOption<ArcaneScroll>(weight: 10),
    //                         AncientOption<MassiveScroll>(weight: 10), // multiplayer only
    //                         AncientOption<ScrollBoxes>(weight: 10),
    //                         AncientOption<WingedBoots>(weight: 10),
    //                         AncientOption<PhialHolster>(weight: 10),
    //                         AncientOption<RejuvenationBead>(),
    //                         AncientOption<SorakasCompassionRelic>()
    //             ),
    //             MakePool(
    //                         AncientOption<TimeCapsule>()
    //             ));
    //
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
                RelicOption<RejuvenationBead>(),
                RelicOption<SorakasCompassionRelic>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<TimeCapsule>(),
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