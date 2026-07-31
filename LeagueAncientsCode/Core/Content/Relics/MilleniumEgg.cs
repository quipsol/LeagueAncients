using BaseLib.Utils;
using GodotPlugins.Game;
using LeagueAncients.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LeagueAncients.Core.Content.Relics;


[Pool(typeof(EventRelicPool))]
public class MillenniumEgg : LeagueAncientsRelicModel
{
  public const string _starterCardKey = "StarterCard";
  public const string _ancientCardKey = "AncientCard";
  public SerializableCard? _serializableStarterCard;
  public SerializableCard? _serializableAncientCard;
  public List<IHoverTip> _extraHoverTips = [];
  
  public override bool HasUponPickupEffect => true;

  public static Dictionary<ModelId, CardModel> TranscendenceUpgrades
  {
    get
    {
      return new Dictionary<ModelId, CardModel>()
      {
        {
          ModelDb.Card<Bash>().Id,
          ModelDb.Card<Break>()
        },
        {
          ModelDb.Card<Survivor>().Id,
          ModelDb.Card<Suppress>()
        },
        {
          ModelDb.Card<Bodyguard>().Id,
          ModelDb.Card<Protector>()
        },
        {
          ModelDb.Card<Venerate>().Id,
          ModelDb.Card<MeteorShower>()
        },
        {
          ModelDb.Card<Zap>().Id,
          ModelDb.Card<Quadcast>()
        }
      };
    }
  }

  public static List<CardModel> TranscendenceCards
  {
    get => MillenniumEgg.TranscendenceUpgrades.Values.ToList<CardModel>();
  }

  public override RelicRarity Rarity => RelicRarity.Ancient;

  [SavedProperty]
  public SerializableCard? StarterCard
  {
    get => this._serializableStarterCard;
    set
    {
      this.AssertMutable();
      this._serializableStarterCard = value;
      UpdateHoverTips();
    }
  }

  [SavedProperty]
  public SerializableCard? AncientCard
  {
    get => this._serializableAncientCard;
    set
    {
      this.AssertMutable();
      this._serializableAncientCard = value;
      UpdateHoverTips();
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    (DynamicVar) new StringVar("StarterCard"),
    (DynamicVar) new StringVar("AncientCard")
  ];

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this._extraHoverTips = new List<IHoverTip>();
  }

  public bool SetupForPlayer(Player player)
  {
    this.AssertMutable();
    CardModel transcendenceStarterCard = this.GetTranscendenceStarterCard(player);
    if (transcendenceStarterCard == null)
      return false;
    this.StarterCard = transcendenceStarterCard.ToSerializable();
    this.AncientCard = this.GetTranscendenceTransformedCard(transcendenceStarterCard).ToSerializable();
    UpdateHoverTips();
    
    
    
    return true;
  }

  public void SetupForTests(SerializableCard starterCard, SerializableCard ancientCard)
  {
    this.AssertMutable();
    this.StarterCard = starterCard;
    this.AncientCard = ancientCard;
    UpdateHoverTips();
  }

  public CardModel? GetTranscendenceStarterCard(Player player)
  {
    return player.Deck.Cards.FirstOrDefault<CardModel>((Func<CardModel, bool>) (c => MillenniumEgg.TranscendenceUpgrades.ContainsKey(c.Id)));
  }

  public CardModel GetTranscendenceTransformedCard(CardModel starterCard)
  {
    CardModel canonicalCard;
    if (!MillenniumEgg.TranscendenceUpgrades.TryGetValue(starterCard.Id, out canonicalCard))
      return (CardModel) this.Owner.RunState.CreateCard<Doubt>(starterCard.Owner);
    CardModel card = starterCard.Owner.RunState.CreateCard(canonicalCard, starterCard.Owner);
    if (starterCard.IsUpgraded)
      CardCmd.Upgrade(card);
    if (starterCard.Enchantment != null)
    {
      EnchantmentModel enchantment = (EnchantmentModel) starterCard.Enchantment.MutableClone();
      CardCmd.Enchant(enchantment, card, (Decimal) enchantment.Amount);
    }
    return card;
  }

  // protected override IEnumerable<IHoverTip> ExtraHoverTips => _extraHoverTips;
  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      UpdateHoverTips();
      return _extraHoverTips;
    }
  }
  // {
  //   get
  //   {
  //     var _extraHoverTips2 = new List<IHoverTip>();
  //
  //     if (StarterCard != null)
  //     {
  //       CardModel cardModel = CardModel.FromSerializable(StarterCard);
  //       _extraHoverTips2.AddRange(cardModel.HoverTips);
  //       _extraHoverTips2.Add(HoverTipFactory.FromCard(cardModel));
  //       ((StringVar) DynamicVars["StarterCard"]).StringValue = cardModel.Title;
  //     }
  //
  //     if (AncientCard != null)
  //     {
  //       CardModel cardModel2 = CardModel.FromSerializable(AncientCard);
  //       _extraHoverTips2.AddRange(cardModel2.HoverTips);
  //       _extraHoverTips2.Add(HoverTipFactory.FromCard(cardModel2));
  //       ((StringVar) DynamicVars["AncientCard"]).StringValue = cardModel2.Title;
  //     }
  //
  //     return _extraHoverTips2;
  //   }
  // }

  public override async Task AfterObtained()
  {
    MillenniumEgg millenniumEgg = this;
    CardModel transcendenceStarterCard = millenniumEgg.GetTranscendenceStarterCard(millenniumEgg.Owner);
    CardPileAddResult? nullable = await CardCmd.Transform(transcendenceStarterCard, millenniumEgg.GetTranscendenceTransformedCard(transcendenceStarterCard));
  }
  
  public void UpdateHoverTips()
  {
    _extraHoverTips.Clear();
    if (StarterCard != null)
    {
      CardModel cardModel = CardModel.FromSerializable(StarterCard);
      _extraHoverTips.AddRange(cardModel.HoverTips);
      _extraHoverTips.Add(HoverTipFactory.FromCard(cardModel));
      ((StringVar)DynamicVars["StarterCard"]).StringValue = cardModel.Title;
    }
    if (AncientCard != null)
    {
      CardModel cardModel2 = CardModel.FromSerializable(AncientCard);
      _extraHoverTips.AddRange(cardModel2.HoverTips);
      _extraHoverTips.Add(HoverTipFactory.FromCard(cardModel2));
      ((StringVar)base.DynamicVars["AncientCard"]).StringValue = cardModel2.Title;
    }
  }
}