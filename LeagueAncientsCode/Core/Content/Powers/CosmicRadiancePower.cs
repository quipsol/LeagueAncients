using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Powers;


public class CosmicRadiancePower : LeagueAncientsPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<IntangiblePower>(1)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature != Owner) return;
        await PowerCmd.Apply<IntangiblePower>(choiceContext,
                    Owner.CombatState!.Allies.Where(c => c is { IsPlayer: true, IsAlive: true }), 
                    DynamicVars[nameof(IntangiblePower)].BaseValue, 
                    Owner, null);
        await PowerCmd.Decrement(this);
    }
}