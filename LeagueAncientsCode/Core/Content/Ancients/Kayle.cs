using BaseLib.Utils;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Core.Content.Ancients;


public class Kayle : LeagueAncientsAncientModel
{
    public override string? CustomMapIconPath => "res://SlayRuneterra/images/placeholder/100_100/purple.png";
    public override string? CustomMapIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";
    public override string? CustomRunHistoryIconPath => "res://SlayRuneterra/images/placeholder/100_100/white.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://SlayRuneterra/images/placeholder/150_150/black.png";
    
    //public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/kayle.tscn";
    
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    //public override bool IsValidForAct(ActModel act) => SlayRuneterraConfig.IsEnabled;

    
    protected override OptionPools MakeOptionPools => new(
                // Divine Feather, Divine Helmet, Divine Idol
                MakePool(
                            AncientOption<DivineFeather>(),
                            AncientOption<DivineHelmet>(),
                            AncientOption<DivineIdol>() 
                ),
                // Flail of Judgement, Sword of Judgement, Spear of Judgement
                MakePool(
                            AncientOption<SpearOfJustice>(),
                            AncientOption<SwordOfJustice>(),
                            AncientOption<FlailOfJustice>()
                ),
                // Essence of an Angel, Ring of Carnage,
                MakePool(
                            AncientOption<RingOfCarnage>(),
                            AncientOption<EssenceOfAnAngel>(),
                            AncientOption<MillenniumEgg>(1, MilleniumEggPrep)
                ));

    private static RelicModel MilleniumEggPrep(MillenniumEgg model)
    {
        var localPlayer = LocalContext.GetMe(RunManager.Instance.DebugOnlyGetState()?.Players ?? []);
        if (localPlayer != null)
            model.SetupForPlayer(localPlayer);
        return model;
    }

    public override Task AfterEventStarted()
    {
        if (!CurrentOptions.Any(o => o.Relic is MillenniumEgg)) return Task.CompletedTask;
        var millenniumEgg = CurrentOptions.First(o => o.Relic is MillenniumEgg).Relic as MillenniumEgg;
        millenniumEgg?.UpdateHoverTips();
        return Task.CompletedTask;
    }

}