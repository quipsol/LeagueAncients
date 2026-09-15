using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using LeagueAncients.Extensions;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace LeagueAncients.Core.Models;


[Pool(typeof(EventRelicPool))]
public abstract class LeagueAncientsRelicModel : CustomRelicModel
{
    //LeagueAncients/images/relics
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}