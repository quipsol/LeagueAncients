using BaseLib.Utils;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Gain Artifact
/// </summary>
[Pool(typeof(EventRelicPool))]
public class BlackShield : LeagueAncientsRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ArtifactPower>(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips  => [HoverTipFactory.FromPower<ArtifactPower>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom) return;
        Flash(); 
        await PowerCmd.Apply<ArtifactPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars[nameof(ArtifactPower)].BaseValue, Owner.Creature, null);
        
    }
}