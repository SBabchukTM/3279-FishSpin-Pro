using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.DevelopingTools
{
    public class DevelopingTools : MonoBehaviour
    {
        [MenuItem("Tools/User Data/Erase All", false, 1)]
        public static void EraseUserData()
        {
            string path = Application.persistentDataPath;

            if (!Directory.Exists(path))
            {
                Debug.Log("No user data found.");
                return;
            }

            if (EditorUtility.DisplayDialog("Erase All User Data",
                    $"This will permanently delete all files in:\n\n{path}\n\nAre you sure?",
                    "Yes, erase everything",
                    "Cancel"))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (FileInfo file in dir.GetFiles())
                        file.Delete();
                    foreach (DirectoryInfo subDir in dir.GetDirectories())
                        subDir.Delete(true);

                    Debug.Log("All user data erased.");
                }
                catch (IOException ex)
                {
                    Debug.LogError($"Failed to erase user data: {ex.Message}");
                }
            }
        }
    }
}
