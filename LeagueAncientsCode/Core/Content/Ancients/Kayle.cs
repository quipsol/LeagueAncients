using BaseLib.Utils;
using Godot;
using LeagueAncients.Core.Content.Relics;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;

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

	public override bool IsValidForAct(ActModel act) => act.Index == 1;
	
/*
	- <Zenith> While you are at or above 70% health, gain 3 Strength and Dexterity.
	
 
 */

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
				RelicOption<Zenith>()
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
