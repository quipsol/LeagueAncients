using BaseLib.Utils;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;

namespace LeagueAncients.Core.Content.Ancients;


public class Kayle : LeagueAncientsAncientModel
{
	public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
	public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
	public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
	public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
	public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/kayle.tscn";

	public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);
	public override Color DialogueColor => new Color("3C1931");

	//public override bool IsValidForAct(ActModel act) => SlayRuneterraConfig.IsEnabled;

	
	// protected override OptionPools MakeOptionPools => new(
	//             // Divine Feather, Divine Helmet, Divine Idol
	//             MakePool(
	//                         AncientOption<DivineFeather>(),
	//                         AncientOption<DivineHelmet>(),
	//                         AncientOption<DivineIdol>() 
	//             ),
	//             // Essence of an Angel, Ring of Carnage, GuidingLight
	//             MakePool(
	//                         AncientOption<RingOfCarnage>(),
	//                         AncientOption<EssenceOfAnAngel>(),
	//                         AncientOption<GuidingLight>()
	//             ),
	//             // Flail of Judgement, Sword of Judgement, Spear of Judgement
	//             MakePool(
	//                         AncientOption<SpearOfJustice>(),
	//                         AncientOption<SwordOfJustice>(),
	//                         AncientOption<FlailOfJustice>()
	//             ));
	//

	public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3];

	private IEnumerable<EventOption> OptionPool1 => [
				RelicOption<DivineFeather>(),
				RelicOption<DivineHelmet>(),
				RelicOption<DivineIdol>(),
	];
	private IEnumerable<EventOption> OptionPool2 => [
				RelicOption<RingOfCarnage>(),
				RelicOption<EssenceOfAnAngelKayle>(),
				RelicOption<GuidingLight>(),
	];
	private IEnumerable<EventOption> OptionPool3 => [
				RelicOption<SpearOfJustice>(),
				RelicOption<SwordOfJustice>(),
				RelicOption<FlailOfJustice>(),
	];
	
	protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
	{
		return 
		[
					Rng.NextItem(OptionPool1)!,
					Rng.NextItem(OptionPool2)!,
					Rng.NextItem(OptionPool3)!,
		];
	}
}
