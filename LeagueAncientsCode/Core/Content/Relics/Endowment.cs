using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace LeagueAncients.Core.Content.Relics;


public class Endowment : LeagueAncientsRelicModel
{
    private const string LEFTOVER_ENERGY = "LeftoverEnergy";
    private const string GRANTED_ENERGY = "GrantedEnergy";
    
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private bool HadLeftoverEnergy
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(LEFTOVER_ENERGY, 1), new EnergyVar(GRANTED_ENERGY, 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => [HoverTipFactory.Static(StaticHoverTip.Energy)];

    public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature)) return Task.CompletedTask;
        HadLeftoverEnergy = Owner.PlayerCombatState!.Energy > 0;
        return Task.CompletedTask;
    }
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!(participants.Contains(Owner.Creature) && HadLeftoverEnergy)) return;
        Flash(); 
        foreach (var player in Owner.Creature.CombatState?.Players.Where(p => p != Owner) ?? []) 
            await PlayerCmd.GainEnergy(DynamicVars[GRANTED_ENERGY].BaseValue, player);
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        HadLeftoverEnergy = false;
        return Task.CompletedTask;
    }
    
}