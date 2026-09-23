using LeagueAncients.Logging;
using MegaCrit.Sts2.Core.Random;

namespace LeagueAncients.Extensions;

public static class RngExtensions
{
    extension(Rng rng)
    {
        /// <summary>
        /// Get unique random items from the specified set of items. <br/>
        /// Will exit early if not enough objects are in the collection.
        /// </summary>
        /// <param name="collection">Set of items to pull from.</param>
        /// <param name="count">How many items you want.</param>
        /// <typeparam name="TItem">Type of items contained in the set.</typeparam>
        /// <returns>Multiple random unique items from the collection.</returns>
        public IEnumerable<TItem> NextItems<TItem>(IEnumerable<TItem> collection, int count)
        {
            if(count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Item count can not be less than zero.");
            var col = collection.ToList();
            HashSet<TItem> items = [];
            for (var i = 0; i < count; i++)
            {
                var validItems = col.Where(t => !items.Contains(t)).ToList();
                if (validItems.Count == 0)
                {
                    ModLog.Info("No more items found.", LogTopic.Logic);
                    break;
                }
                items.Add(rng.NextItem(validItems)!);
            }
            return items;
        }
    }
}