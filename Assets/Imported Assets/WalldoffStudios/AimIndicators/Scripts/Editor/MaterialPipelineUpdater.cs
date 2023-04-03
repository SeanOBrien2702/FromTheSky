#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace WalldoffStudios.Utils
{
    public static class MaterialPipelineUpdater
    {
        [MenuItem("WalldoffStudios/Set scene materials pipeline/Built in")]
        static void SetBuiltInPipeline()
        {
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/WalldoffStudios/AimIndicators/Materials/SceneItems" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                
                material.shader = Shader.Find("Standard"); 
                EditorUtility.SetDirty(material);
            }
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("WalldoffStudios/Set scene materials pipeline/URP")]
        static void SetUniversalPipeline()
        {
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/WalldoffStudios/AimIndicators/Materials/SceneItems" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                
                material.shader = Shader.Find("Universal Render Pipeline/Lit"); 
                EditorUtility.SetDirty(material);
            }
            AssetDatabase.SaveAssets();
        }
        
        [MenuItem("WalldoffStudios/Set scene materials pipeline/HDRP")]
        static void SetHighDefinitionPipeline()
        {
            string[] guids = AssetDatabase.FindAssets("t:Material", new[] { "Assets/WalldoffStudios/AimIndicators/Materials/SceneItems" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
                
                material.shader = Shader.Find("HDRP/Lit"); 
                EditorUtility.SetDirty(material);
            }
            AssetDatabase.SaveAssets();
        }
    }   
}
#endif
