using BaseLib.Utils;
using LeagueAncients.Core.Content.Powers;
using LeagueAncients.Core.Models;
using LeagueAncients.Util;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Gain Strength and Dexterity at high health
/// </summary>
[Pool(typeof(EventRelicPool))]
public class Zenith : LeagueAncientsRelicModel
{
    private const string THRESHOLD = "Threshold";
    private const string THRESHOLD_CALC = "CalculatedThreshold";
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(3), new PowerVar<DexterityPower>(1), 
                new (THRESHOLD, 70),
                .. new CalculatedRelicVar(THRESHOLD_CALC)
                            .WithMultiplier(relic => ((Zenith?)relic)?.ThresholdCalculated ?? 0)
                            .FinalizeWithVars(0, 1)];

    private bool _active;
    public bool WithinThreshold => Owner.Creature.CurrentHp >= ThresholdCalculated;
    private decimal ThresholdCalculated => Owner.Creature.MaxHp * DynamicVars[THRESHOLD].BaseValue / 100;
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom) return;
        _active = false;
        if(!WithinThreshold) return;
        Flash();
        await PowerCmd.Apply<ZenithPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
        _active = true;
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        ((StringVar)DynamicVars[THRESHOLD_CALC]).StringValue = $"{ThresholdCalculated}";
        if (!CombatManager.Instance.IsInProgress) return;
        var withinThreshold = WithinThreshold;
        if (withinThreshold && !_active)
        {
            Flash();
            await PowerCmd.Apply<ZenithPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
        }
        _active = withinThreshold;
    }
}