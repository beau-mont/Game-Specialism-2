using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FactoryController
{
    private static List<IWaveFactory> factories = new List<IWaveFactory>();
    public static IWaveFactory GetFactory(string factoryType)
    {
        if (factories.Count == 0)
        {
            Type[] types = Assembly.GetAssembly(typeof(IWaveFactory)).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(IWaveFactory))).ToArray();
            foreach (Type t in types)
            {
                IWaveFactory factory = ScriptableObject.CreateInstance(t) as IWaveFactory;

                if (!factory)
                {
                    Debug.LogError("Found null factory");
                    continue;
                }
                factories.Add(factory);
                Debug.Log("Registered factory: " + factory.FactoryName);
            }
        }
        foreach (var factory in factories) 
        { 
            if (factory.FactoryName == factoryType) return factory; // if found, return factory
        } 
        Debug.LogError($"No factory of type {factoryType} found.");
        return null; // if no factory found, return null
    }
}
