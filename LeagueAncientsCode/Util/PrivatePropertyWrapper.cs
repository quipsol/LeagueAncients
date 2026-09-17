using System.Reflection;
using HarmonyLib;
using LeagueAncients.Extensions;

namespace LeagueAncients.Util;


// Based on Pikcube's versions:
// https://github.com/pikcube/Pikcube.Common/blob/master/Utility/PrivateWrapper.cs


// Caches to minimize AccessTools usage. Exclusive access to this file.
// Not inside the structs because typed structs would create a new Dictionary for every <TParent, T> pairing which is terrible.
// If I access <RunManager, RunState>("State") and <RunManager, RunHistory>("History") it would create two Dictionaries.
// Even though we don't really care about the "RunState/RunHistory" as they are exclusively used for casting the return value,
// they are part of the generic structure and would result in two Dictionaries, each holding one entry.
file static class Caches
{
    internal static readonly Dictionary<(Type Type, string Name), PropertyInfo> PropetyCache = new();
    internal static readonly Dictionary<(Type Type, string Name), FieldInfo> FieldCache = new();
}

public  readonly struct  PrivatePropertyWrapper<TParent, T>(TParent parent, string name)
{
    
    private readonly PropertyInfo _propertyInfo = Caches.PropetyCache.GetOrCreate(
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