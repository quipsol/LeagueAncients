using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using LeagueAncients.Core.Multiplayer;
using LeagueAncients.Extensions;
using LeagueAncients.Util;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Core.Content.Ancients;

// Sword of Blossoming Dawn. MP only. Deal damage, heal the ally with the lowest hp for damage dealt. Exhaust.

public class Taric : LeagueAncientsAncientModel
{
    public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/taric.tscn";

    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
    public override Color DialogueColor => new Color("3C1931");

    public override bool IsValidForAct(ActModel act) => !RunConfigSnapshot.Active.DisableLeagueAncients && GameHelper.GetRunState().IsMultiplayer;
    
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3, RingOfFriendship];

    private IEnumerable<EventOption> OptionPool1 => [
                RelicOption<CosmicRadiance>(),
                RelicOption<Shareholder>(),
    ];
    private IEnumerable<EventOption> OptionPool2 => [
                RelicOption<SwordOfBlossomingDawn>(),
                RelicOption<Bravado>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<LocketOfTheIronSolari>(),
    ];

    /// Either offered to ALL or NONE
    private EventOption RingOfFriendship => RelicOption<RingOfFriendship>();
    /// Only offered to a maximum of two players (does not mean it is guaranteed!)
    private EventOption Endowment => RelicOption<Endowment>();
    
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        // Using the Niche rng here and not the event one, because the niche will be the same result for everyone.
        var nicheRng = Owner!.RunState.Rng.Niche;
        
        var playersValidForEndowment = nicheRng.NextItems(Owner!.RunState.Players.ToList(), 2);
        var pool2 = OptionPool2.ToList();
        if(playersValidForEndowment.Contains(Owner)) 
            pool2.Add(Endowment);
        
        return 
        [
                    Rng.NextItem(OptionPool1)!,
                    Rng.NextItem(pool2)!,
                    Owner!.RunState.CurrentActIndex == 1 &&  nicheRng.NextBool() ? RingOfFriendship : Rng.NextItem(OptionPool3)!,
        ];
    }
}