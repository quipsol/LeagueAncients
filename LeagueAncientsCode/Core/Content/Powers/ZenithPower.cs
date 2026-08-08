using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace LeagueAncients.Core.Content.Powers;


public class ZenithPower : LeagueAncientsPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

   
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(3), new PowerVar<DexterityPower>(1)];
    
    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        var zenith = Owner.Player?.Relics.FirstOrDefault(r => r is Zenith) as Zenith ?? null;
        if (!zenith?.WithinThreshold ?? true)
        {
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, -DynamicVars.Strength.BaseValue, Owner, null);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner, -DynamicVars.Dexterity.BaseValue, Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars.Strength.BaseValue, Owner, null);
        await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars.Dexterity.BaseValue, Owner, null);
    }
}