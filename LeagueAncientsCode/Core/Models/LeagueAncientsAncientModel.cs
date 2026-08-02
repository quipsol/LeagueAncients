using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Events;

namespace LeagueAncients.Core.Models;

public abstract class LeagueAncientsAncientModel : CustomAncientModel
{
    protected sealed override OptionPools MakeOptionPools => throw new  NotImplementedException("This should not be accessed");

    public sealed override IEnumerable<EventOption> AllPossibleOptions => AllPossibleEventOptions;
    /// <summary>
    /// Is <see cref="AllPossibleOptions"/>
    /// </summary>
    public abstract IEnumerable<EventOption> AllPossibleEventOptions { get; }
    
    protected sealed override IReadOnlyList<EventOption> GenerateInitialOptions() => GenerateInitialEventOptions();
    
    /// <summary>
    /// Is <see cref="GenerateInitialOptions"/>
    /// </summary>
    protected abstract IReadOnlyList<EventOption> GenerateInitialEventOptions();
}