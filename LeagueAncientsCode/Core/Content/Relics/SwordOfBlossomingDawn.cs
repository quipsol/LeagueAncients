using LeagueAncients.Core.Models;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LeagueAncients.Core.Content.Relics;


public class SwordOfBlossomingDawn : LeagueAncientsRelicModel
{
    private const string ATTACK_THRESHOLD = "AttackThreshold";
    
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool ShowCounter => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new (ATTACK_THRESHOLD, 10), new HealVar(4)];
    
    public override int DisplayAmount => (IsActivating) ? DynamicVars[ATTACK_THRESHOLD].IntValue : AttacksPlayed % DynamicVars[ATTACK_THRESHOLD].IntValue;

    private bool IsActivating
    {
        get;
        set
        {
            AssertMutable();
            field = value;
            UpdateDisplay();
        }
    }
    
    [SavedProperty]
    public int AttacksPlayed
    {
        get;
        private set
        {
            AssertMutable();
            field = value % DynamicVars[ATTACK_THRESHOLD].IntValue;
            UpdateDisplay();
        }
    }
    
    private void UpdateDisplay()
    {
        if (IsActivating) Status = RelicStatus.Normal;
        else Status = ((AttacksPlayed == 9) ? RelicStatus.Active : RelicStatus.Normal);
        InvokeDisplayAmountChanged();
    }
    
    private void NotifyAttackPlayed()
    {
        if (++AttacksPlayed == 0) TaskHelper.RunSafely(DoActivateVisuals());
    }
    
    // This feels wrong. It's not tied to the actual execution logic. But the game does the same for PenNib.
    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack) return;
        if (cardPlay.Card.Owner != Owner) return;
        
        NotifyAttackPlayed();
        if (AttacksPlayed != 0) return;
        var player = Owner.RunState.Players.Where(p => p != Owner).MinBy(p => p.Creature.CurrentHp);
        if(player != null) 
            await CreatureCmd.Heal(player.Creature, DynamicVars.Heal.BaseValue);
    }
}