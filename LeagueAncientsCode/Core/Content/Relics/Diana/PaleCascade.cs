using LeagueAncients.Core.Content.Powers;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// If you took damage, gain Block at the start of next turn
/// </summary>
public class PaleCascade : LeagueAncientsRelicModel
{
    private const string TRIGGERS = "Triggers";
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private int _counter;

    public override bool ShowCounter => Owner.Creature.CombatState is not null;
    public override int DisplayAmount => _counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new (TRIGGERS, 3), new PowerVar<PaleCascadePower>(10)];

    public override Task BeforeCombatStart()
    {
        _counter = DynamicVars[TRIGGERS].IntValue;
        return  Task.CompletedTask;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner.Creature || !props.IsPoweredAttack() || result.UnblockedDamage <= 0) return;
        if (--_counter >= 0)
        {
            Flash();
            await PowerCmd.Apply<PaleCascadePower>(choiceContext, Owner.Creature, DynamicVars[nameof(PaleCascadePower)].BaseValue, Owner.Creature, null);
        }
    }
}