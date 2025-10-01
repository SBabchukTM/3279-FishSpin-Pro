using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.AdaptiveIcon
{
    public class AdaptiveIconGenerator : EditorWindow
    { 
        private Texture2D sourceIcon;

        [MenuItem("Tools/Adaptive Icon Generator")]
        public static void ShowWindow()
        {
            GetWindow<AdaptiveIconGenerator>("Adaptive Icon Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Source Icon", EditorStyles.boldLabel);
            sourceIcon = (Texture2D)EditorGUILayout.ObjectField("Icon", sourceIcon, typeof(Texture2D), false);

            if (sourceIcon != null && GUILayout.Button("Generate Adaptive Icon"))
            {
                GenerateAdaptiveIcon();
            }
        }

        private void GenerateAdaptiveIcon()
        {
            string path = AssetDatabase.GetAssetPath(sourceIcon);
            string directory = Path.GetDirectoryName(path);
            string outputPath = Path.Combine(directory, "adaptive_icon.png");

            // Resize to 344x344
            Texture2D resizedIcon = ResizeTexture(sourceIcon, 344, 344);

            // Create transparent 512x512
            Texture2D finalTexture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
            Color32[] transparent = new Color32[512 * 512];
            for (int i = 0; i < transparent.Length; i++) transparent[i] = new Color32(0, 0, 0, 0);
            finalTexture.SetPixels32(transparent);

            // Paste resizedIcon into center of finalTexture
            int startX = (512 - 344) / 2;
            int startY = (512 - 344) / 2;
            for (int x = 0; x < 344; x++)
            {
                for (int y = 0; y < 344; y++)
                {
                    Color color = resizedIcon.GetPixel(x, y);
                    finalTexture.SetPixel(startX + x, startY + y, color);
                }
            }

            finalTexture.Apply();

            // Save to disk
            byte[] pngData = finalTexture.EncodeToPNG();
            File.WriteAllBytes(outputPath, pngData);
            AssetDatabase.Refresh();

            Debug.Log($"Adaptive icon saved to: {outputPath}");
        }

        private Texture2D ResizeTexture(Texture2D source, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 0);
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();
            RenderTexture.active = null;
            rt.Release();
            return result;
        }
    }
}
