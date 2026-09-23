using System.Reflection;
using HarmonyLib;
using LeagueAncients.Extensions;

namespace LeagueAncients.Util;


// Based on Pikcube's versions:
// https://github.com/pikcube/Pikcube.Common/blob/d35267eb5beb5f92dc6fc0ed5b62a5f3fd381395/Utility/PrivateWrapper.cs


// Caches to minimize AccessTools usage. Exclusive access to this file.
// Not inside the structs because typed structs would create a new Dictionary for every <TParent, T> pairing which is terrible.
// Even though we don't really care about the "T" as it is exclusively used for casting the return value,
// it is part of the generic structure and would result multiple dictionaries even when accessing the same parent class.
file static class Caches
{
    internal static readonly Dictionary<(Type Type, string Name), PropertyInfo> PropertyCache = new();
    internal static readonly Dictionary<(Type Type, string Name), FieldInfo> FieldCache = new();
}

/// <summary>
/// A simple-to-use wrapper for accessing private properties.
/// </summary>
/// <param name="parent">The object instance (if the property isn't static).</param>
/// <param name="name">The property's name.</param>
/// /// <typeparam name="TParent">The type of the class where the property is defined.</typeparam>
/// <typeparam name="T">The type of the property.</typeparam>
public  readonly struct  PrivatePropertyWrapper<TParent, T>(TParent parent, string name)
{
    
    private readonly PropertyInfo _propertyInfo = Caches.PropertyCache.GetOrCreate(
                (Type: typeof(TParent), Name: name), 
                key => AccessTools.DeclaredProperty(key.Type, key.Name) 
                       ?? throw new MissingMemberException(key.Type.FullName, key.Name));

    /// <summary>The property's value.</summary>
    public T? Value
    {
        get => (T?)_propertyInfo.GetValue(parent);
        set => _propertyInfo.SetValue(parent, value);
    }
}

/// <summary>
/// A simple-to-use wrapper for accessing private fields.
/// </summary>
/// <param name="parent">The object instance (if the field isn't static).</param>
/// <param name="name">The field's name.</param>
/// /// <typeparam name="TParent">The type of the class where the field is defined.</typeparam>
/// <typeparam name="T">The type of the field.</typeparam>
public readonly struct PrivateFieldWrapper<TParent, T>(TParent? parent, string name)
{
    private readonly FieldInfo _fieldInfo  = Caches.FieldCache.GetOrCreate(
                (Type: typeof(TParent), Name: name), 
                key => AccessTools.DeclaredField(key.Type, key.Name) 
                       ?? throw new MissingMemberException(key.Type.FullName, key.Name));

    /// <summary>The field's value.</summary>
    public T? Value
    {
        get => (T?)_fieldInfo.GetValue(parent);
        set => _fieldInfo.SetValue(parent, value);
    }
}