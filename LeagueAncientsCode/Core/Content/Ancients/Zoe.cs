using Godot;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace LeagueAncients.Core.Content.Ancients;


// Gives options from all other ancients (and maybe also rare relics from other characters?)
// Add custom ancient card that turns into a random ancient card at the start of combat (and enchants them with something, idk)

//  Total pools: 5
// Pool 1 - 3: A random relic from other ancients (in full 3-act mod only from other Targonian ancients)
// Pool 4: Two random character specific relics from the Rare category
// Pool 5: A few Zoe specific options


/*
 
 What makes Zoe special?
  1. Can appear in Act 2 or 3
  2. Has more than 3 Options

 One of the pools is only if Zoe is an Act3 Event. It specifcally grabs one option from the Ancient you have seen in Act 2, which isn't the option chosen
   Concerns: Act 2 Ancient relics are a bit weaker than Act 3. Maybe make it 2 and have them be random?
 
 
 
 Act 2 only. Prismatic Gem but instead of 1 bonus energy, cards from other characters have their cost reduced by 1!? (Maybe only the first played each turn)
   Should also immediately give you some
 
 Gain an additional Ancient potion reward at the end of Elite combats.
   Need to make Ancient Potion Rarity and the potions. They should be something special.
   Issues: Potions are already very warping. Making even stronger ones might be a bad idea. Counterpoint: Ancient cards exist, and they are often also very warping.
   But: I want them all to be very interesting and special. No "Get 5 Strength" ahh shit.
 
 
 */




public class Zoe : LeagueAncientsAncientModel
{
    public override string CustomMapIconPath => "res://LeagueAncients/images/placeholder/100_100/purple.png";
    public override string CustomMapIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";
    public override string CustomRunHistoryIconPath => "res://LeagueAncients/images/placeholder/100_100/white.png";
    public override string CustomRunHistoryIconOutlinePath => "res://LeagueAncients/images/placeholder/150_150/black.png";

    //public override string? CustomBackgroundScenePath => "res://SlayRuneterra/scenes/events/background_scenes/zoe.tscn";
    public override string CustomScenePath => "res://LeagueAncients/scenes/events/background_scenes/zoe.tscn";
    
    public override bool IsValidForAct(ActModel act) => !Config.DisableLeagueAncients && act.Index is 1 or 2;


    
    public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);

    public override Color DialogueColor => new Color("3C1931");
    
    
    public override IEnumerable<EventOption> AllPossibleEventOptions => [..OptionPool1, ..OptionPool2, ..OptionPool3, ..SeaGlassOptions];

    private IEnumerable<EventOption> OptionPool1 => [
                RelicOption<Orrery>(),
                RelicOption<PrismaticGem>(),
    ];
    private IEnumerable<EventOption> OptionPool2 => [
                RelicOption<Astrolabe>(),
                RelicOption<Driftwood>(),
    ];
    private IEnumerable<EventOption> OptionPool3 => [
                RelicOption<CharonsAshes>(),
                RelicOption<OrangeDough>(),
                RelicOption<PowerCell>(),
                RelicOption<PaperKrane>(),
                RelicOption<BigHat>(),
    ];
    
    private IEnumerable<EventOption> SeaGlassOptions
    {
        get
        {
           var list = new List<EventOption>();
            foreach (var characterModel in ModelDb.AllCharacters)
            {
                var seaGlass = (SeaGlass)ModelDb.Relic<SeaGlass>().ToMutable();
                seaGlass.CharacterId = characterModel.Id;
                list.Add(RelicOption(seaGlass));
            }
            return list;
        }
    }
    
    protected override IReadOnlyList<EventOption> GenerateInitialEventOptions()
    {
        var ownerCharacterModel = Owner!.Character;
        var characterModel = Rng.NextItem(Owner.UnlockState.Characters.Where(c => c.Id != ownerCharacterModel.Id)) ?? ownerCharacterModel;
        var list = OptionPool1.ToList();
        var seaGlass = (SeaGlass)ModelDb.Relic<SeaGlass>().ToMutable();
        seaGlass.CharacterId = characterModel.Id;
        list.Add(RelicOption(seaGlass));
        return 
        [
                    Rng.NextItem(list)!,
                    Rng.NextItem(list)!,
                    Rng.NextItem(OptionPool2)!,
                    Rng.NextItem(OptionPool3)!,
        ];
    }
}