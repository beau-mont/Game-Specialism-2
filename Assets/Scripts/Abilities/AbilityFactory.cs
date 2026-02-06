using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Reflection;
using UnityEditor;

public static class AbilityFactory
{
    private static IAbility[] GetAllAbilities()
    {
        return AssetDatabase.FindAssets("t:IAbility") // find all assets of type IAbility
            .Select(guid => AssetDatabase.LoadAssetAtPath<IAbility>(AssetDatabase.GUIDToAssetPath(guid))) // load each asset
            .Where(ability => ability != null && ability.GetType().IsSubclassOf(typeof(IAbility))) // filter out nulls and non-IAbility types
            .ToArray(); // convert to array
    }

    public static bool TryGetAbility(string abilityName, out IAbility ability)
    {
        ability = GetAllAbilities().FirstOrDefault(ability => ability.AbilityName == abilityName);
        return ability != null;
    }
}
