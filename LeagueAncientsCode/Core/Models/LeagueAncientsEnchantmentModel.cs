using BaseLib.Abstracts;

namespace LeagueAncients.Core.Models;

public abstract class LeagueAncientsEnchantmentModel : CustomEnchantmentModel
{
    // TODO: Change to path based on class name
    protected override string CustomIconPath => "res://LeagueAncients/images/placeholder/100_100/blue.png";
    public override bool HasExtraCardText => false;
    public override bool ShowAmount => false;
}