using System.IO;
using UnityEngine;

namespace Services
{
    public class FileHandler
    {
        private string GetFullPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        public bool Exists(string fileName)
        {
            return File.Exists(GetFullPath(fileName));
        }

        public string Read(string fileName)
        {
            return File.ReadAllText(GetFullPath(fileName));
        }

        public void Write(string fileName, string content)
        {
            File.WriteAllText(GetFullPath(fileName), content);
        }
    }
}