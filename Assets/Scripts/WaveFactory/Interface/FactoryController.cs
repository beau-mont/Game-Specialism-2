using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A static class that provides a static method to recall a wave factory by name from the asset list.
/// </summary>
public static class FactoryController
{
    private static List<IWaveFactory> factories = new List<IWaveFactory>();
    public static IWaveFactory GetFactory(string factoryType)
    {
        if (factories.Count == 0)
        {
            factories = AssetDatabase.FindAssets("t:IWaveFactory") // find all assets of type IWaveFactory
            .Select(guid => AssetDatabase.LoadAssetAtPath<IWaveFactory>(AssetDatabase.GUIDToAssetPath(guid))) // load each asset
            .Where(factory => factory != null && factory.GetType().IsSubclassOf(typeof(IWaveFactory))) // filter out nulls and abstract classes
            .ToList(); // convert to list
        }
        IWaveFactory fac = factories.Where(factory => factory.FactoryName == factoryType).FirstOrDefault();
        if (fac) return fac;
        else
        Debug.LogError($"No factory of type {factoryType} found.");
        return null; // if no factory found, return null
    }
}
