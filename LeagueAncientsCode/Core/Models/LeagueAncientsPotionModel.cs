using BaseLib.Abstracts;
using BaseLib.Utils;
using LeagueAncients.Extensions;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models.PotionPools;

namespace LeagueAncients.Core.Models;

[Pool(typeof(SharedPotionPool))]
public abstract class LeagueAncientsPotionModel : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Ancient;
}


/*
Potion drop chance
50% Normal
75% Elite

Potion rarity chance
10% Rare
25% Uncommon
65% Common

The chance is specifically filited, adding new Rarities will not appear here by default.



Rare potions:
Enemy's attacks deal 30% less damage for 4 turns
Shuffle ALL your cards into your Draw Pile. Draw 5.
Play the top 3 cards of your Draw Pile.
Choose a card in your Draw Pile and add it to your Hand.
Fill all your empty potion slots with random potions.
When your would die, revive with 30% hp.
Gain 5 max hp.
Next attack deals triple damage.
Put a card from Discard Pile into Hand. It is free this turn.
Gain 1 Buffer.
Gain 1 Ritual.
Add a random Attack, Skill and Power into your Hand. They are free this turn.
All enemies lose 7 Strength this turn.
Gain 10 Block. Next turn gain 10 Block.
Draw 7 cards. Randomize the cost of hand cards this turn.

Ancient potions (every potion has 1 Ancient associated with them):
Gain 1 Intangible (Taric)
Gain 1 Artifact (Morgana) (On demand Artifact, even just once compared to the relic, is too busted!)
All enemies take double damage this turn (Kayle)
Fill all your empty potion slots with Rare or Ancient potions (Zoe)
Gain reverse hunter killer buff for the rest of combat
Upgrade all cards in your deck for the rest of combat

 */