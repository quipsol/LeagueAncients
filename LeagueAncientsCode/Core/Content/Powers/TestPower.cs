using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace LeagueAncients.Core.Content.Powers;



public class TestPower : LeagueAncientsPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;



    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        AbstractRoom currentRoom = base.CombatState.RunState.CurrentRoom!;
        if(currentRoom is not CombatRoom combatRoom) return;
        combatRoom.AddExtraReward(Owner.Player!, new RelicReward(ModelDb.Relic<BlackShield>().ToMutable(), Owner.Player!));

    }

}