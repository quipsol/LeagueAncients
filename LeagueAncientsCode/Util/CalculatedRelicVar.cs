using System.Globalization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LeagueAncients.Util;

public class CalculatedRelicVar(string name) : DynamicVar(name, 0M)
{
  private Func<RelicModel, decimal>? _multiplierCalc;
  
  
  public override void SetOwner(AbstractModel owner)
  {
    base.SetOwner(owner);
    UpdateValues();
  }

  /// <summary>
  /// Should be called last when initializing.<br/>
  /// Creates Base and Extra vars using the name + "Base" and "Extra" suffix.
  /// </summary>
  /// <param name="baseValue">Add a base value to every result</param>
  /// <param name="extraValue">Multiply every result by this</param>
  /// <returns>Returns the CalculatedVar, the BaseVar, and the ExtraVar</returns>
  public IEnumerable<DynamicVar> FinalizeWithVars(decimal baseValue, decimal  extraValue)
  {
    var baseDynVar = new DynamicVar($"{Name}Base", baseValue);
    var extraDynVar = new DynamicVar($"{Name}Extra", extraValue);
    return [this, baseDynVar, extraDynVar];
  }
  


  /// <summary>
  /// Set the function that will be used for the multiplier value of this var.
  /// </summary>
  public CalculatedRelicVar WithMultiplier(Func<RelicModel, decimal> multiplierCalc)
  {
    if (_multiplierCalc is not null)
      throw new InvalidOperationException($"Tried to set extra multiplier calc on {this} twice!");
    _multiplierCalc = multiplierCalc.Target is not AbstractModel ? multiplierCalc : throw new InvalidOperationException("Multiplier calc must be static!");
    return this;
  }

  
  public decimal Calculate()
  {
    if (_multiplierCalc is null)
      throw new InvalidOperationException("Extra multiplier calc must be specified!");
    var num = _multiplierCalc((RelicModel) _owner!);
    return GetBaseVar().BaseValue + GetExtraVar().BaseValue * num;
  }

  public void RecalculateForUpgradeOrEnchant()
  {
    var baseValue = GetBaseVar().BaseValue;
    if (baseValue != BaseValue)
      WasJustUpgraded = true;
    BaseValue = baseValue;
  }

  public void UpdatePreviewVar(bool runGlobalHooks)=> PreviewValue = Calculate();
  
  /// <summary>
  /// Get the DynamicVar that should be used for this calculation's base value.
  /// </summary>
  protected virtual DynamicVar GetBaseVar() => ((RelicModel)_owner!).DynamicVars[$"{Name}Base"];
  

  /// <summary>
  /// Get the DynamicVar that should be used for this calculation's extra value.
  /// </summary>
  protected virtual DynamicVar GetExtraVar() => ((RelicModel)_owner!).DynamicVars[$"{Name}Extra"];
  
  
  /// <inheritdoc/>
  protected override decimal GetBaseValueForIConvertible() => Calculate();

  /// <inheritdoc/>
  public override string ToString() => Calculate().ToString(CultureInfo.InvariantCulture);

  private void UpdateValues()
  {
    if (_owner is null) return;
    BaseValue = GetBaseVar().BaseValue;
  }
}