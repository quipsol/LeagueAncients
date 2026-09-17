using System.Diagnostics;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using LeagueAncients.Core.Multiplayer;
using LeagueAncients.Extensions;
using LeagueAncients.Util;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace LeagueAncients.Core.Content.Ancients;

// Sword of Blossoming Dawn. MP only. Deal damage, heal the ally with the lowest hp for damage dealt. Exhaust.

public class Taric : LeagueAncientsAncientModel
{
    public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/kayle.tscn";

    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => !RunConfigSnapshot.Active.DisableLeagueAncients && GameHelper.GetRunState().IsMultiplayer && Owner.RunState.IsMultiplayer;
    
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3, RingOfFriendship];

    private IEnumerable<EventOption> OptionPool1 => [
                RelicOption<CosmicRadiance>(),
                RelicOption<Endowment>(),
                RelicOption<Shareholder>(),
    ];
    private IEnumerable<EventOption> OptionPool2 => [
                RelicOption<SwordOfBlossomingDawn>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<LocketOfTheIronSolari>(),
    ];

    // Either offered to ALL or NONE
    private EventOption RingOfFriendship => RelicOption<RingOfFriendship>();
    
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        // TODO: Handle Ring Of Friendship
        return 
        [
                    Rng.NextItem(OptionPool1)!,
                    Rng.NextItem(OptionPool2)!,
                    Rng.NextItem(OptionPool3)!,
        ];
    }
}