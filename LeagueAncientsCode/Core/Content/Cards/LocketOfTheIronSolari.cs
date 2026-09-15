using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace LeagueAncients.Core.Content.Cards;


public class LocketOfTheIronSolari() : LeagueAncientsCardModel(1, CardType.Skill, CardRarity.Ancient, TargetType.AllAllies)
{
    // public override string BetaPortraitPath => "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string PortraitPath => null!;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";
    // public override string? CustomPortraitPath => null;// "res://LeagueAncients/images/card_portraits/colorless_ancient_placeholder.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var player in Owner.Creature.CombatState!.Allies.Where(c => c is {IsPlayer: true, IsAlive: true}))
            await CreatureCmd.GainBlock(player, DynamicVars.Block, play);
    }

    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3);
    
}