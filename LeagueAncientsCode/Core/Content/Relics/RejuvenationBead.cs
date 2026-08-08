using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Heal to full HP the first time you enter a Rest Site
/// </summary>
[Pool(typeof(EventRelicPool))]
public class RejuvenationBead : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override bool IsUsedUp => HasTriggered;
    
    private bool _hasTriggered;
    
    [SavedProperty]
    public bool HasTriggered
    {
        get =>  _hasTriggered;
        set
        {
            AssertMutable();
            _hasTriggered = value;
            if(_hasTriggered)
                Status = RelicStatus.Disabled;
            InvokeDisplayAmountChanged();
        }
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (HasTriggered || room is not RestSiteRoom)
            return;
        Flash();
        await CreatureCmd.Heal(Owner.Creature, Owner.Creature.MaxHp);
        Status = RelicStatus.Disabled;
        HasTriggered = true;
    }
}