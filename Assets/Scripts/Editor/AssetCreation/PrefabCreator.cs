using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.AssetCreation
{
    public class PrefabCreator
    { 
        public static string Create(string className, string namespacePath, GameObject prefab, string savePath)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var type = FindTypeInAssemblies($"{namespacePath}.{className}");
            var component = instance.AddComponent(type);
            
            string path = Path.Combine(savePath, $"{className}.prefab");
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);

            return path;
        }
        
        private static Type FindTypeInAssemblies(string fullClassName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(fullClassName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}