using BaseLib.Utils;
using LeagueAncients.Core.Models;
using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace LeagueAncients.Core.Content.Relics;

/// <summary>
/// Get the two other offers before the boss fight
/// </summary>
public class DesperateBargain : LeagueAncientsRelicModel
{
    private const string RELIC_TITLES = "RelicTitles";
    private const string SERIALIZABLE_RELIC = "SerializableRelics";
    
    // All of this just because I cant simply use SavedProperty on a List<SerializableRelic>
    private static readonly SavedSpireField<DesperateBargain, List<SerializableRelic>> SavedRelics =
                new(() => [], SERIALIZABLE_RELIC)
                {
                            Serializer = (list, writer) =>
                            {
                                writer.WriteInt(list.Count);
                                foreach (var r in list) r.Serialize(writer);
                            },
                            Deserializer = reader =>
                            {
                                var count = reader.ReadInt();
                                var list = new List<SerializableRelic>(count);
                                for (var i = 0; i < count; i++)
                                {
                                    var r = new SerializableRelic();
                                    r.Deserialize(reader);
                                    list.Add(r);
                                }
                                return list;
                            }
                };
    
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar(RELIC_TITLES)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => SerializableRelicsToHoverTips();


    public List<SerializableRelic> SerializableRelics
    {
        get => SavedRelics.Get(this)!;
        private set
        {
            AssertMutable();
            SavedRelics.Set(this, value);
            UpdateRelicList();
        }
    }
    


    public override Task AfterObtained()
    {
        UpdateRelicList();
        return Task.CompletedTask;
    }

    
    
    // Multiplayer issue!
    // If the player is currently dead, "RewardsSet.Offer()" will skip itself.
    // set a flag if the player reached the correct campsite
    // Then on every campsite enter check if they are alive. Additionally, check if they got revived!
    // Until the rewards are offered.
    
    
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room.RoomType is not RoomType.RestSite || SerializableRelics.Count == 0) return Task.CompletedTask;
        // TODO: Make sure im in the latest RestSite before boss
        
        
        var relics = SerializableRelics.Select(sr => new RelicReward(FromSerializable(sr), Owner))
                    .Cast<Reward>().ToList();

        // If we await RewardsCmd here in async, the fade in for RestSites will indefinitely bock the screen and you softlock.
        // To prevent this, the RewardsCmd is initiated with the TaskHelper so the Room entering logic can continue and finish.
        TaskHelper.RunSafely(RewardsCmd.OfferCustom(Owner, relics));
        SerializableRelics.Clear();
        UpdateRelicList();
        return Task.CompletedTask;
    }
    
    
    private void UpdateRelicList()
    {
        Status = SerializableRelics.Count <= 0 ? RelicStatus.Disabled : RelicStatus.Normal;
        var stringVar = (StringVar)DynamicVars[RELIC_TITLES];
        stringVar.StringValue = SerializableRelics.Count == 0 
                    ? string.Empty
                    : string.Join('\n', SerializableRelics.Select(c => "- " + SaveUtil.RelicOrDeprecated(c.Id!).Title));
        InvokeDisplayAmountChanged();
    }

    private IEnumerable<IHoverTip> SerializableRelicsToHoverTips()
        => SerializableRelics.SelectMany(sr => HoverTipFactory.FromRelic(FromSerializable(sr)));
    
    public void SetRewardRelics(IEnumerable<RelicModel?> relics) 
        => SerializableRelics = relics.OfType<RelicModel>().Select(r => r.ToSerializable()).ToList();
   
    
    public void DebugAddRelic(SerializableRelic relic) => SerializableRelics.Add(relic);
}