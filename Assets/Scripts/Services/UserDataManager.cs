using System;
using UnityEngine;

namespace Services
{
    public class UserDataManager
    {
        private readonly FileHandler _fileHandler;
        private readonly JsonSerializer<UserData> _serializer;

        private UserData _userData;
        private const string FileName = "userdata.json";

        public UserDataManager()
        {
            _fileHandler = new FileHandler();
            _serializer = new JsonSerializer<UserData>();
        }

        public UserData GetData() => _userData;

        public void Load()
        {
            if (!_fileHandler.Exists(FileName))
            {
                Debug.Log("[UserDataManager] Save file not found. Creating new UserData.");
                _userData = new UserData();
                return;
            }

            try
            {
                string json = _fileHandler.Read(FileName);
                Debug.Log($"[UserDataManager] Loaded JSON: {json}");

                _userData = _serializer.Deserialize(json) ?? new UserData();
                Debug.Log("[UserDataManager] Load successful.");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[UserDataManager] Failed to load file. Using new UserData. Error: {e}");
                _userData = new UserData();
            }
        }

        public void Save()
        {
            try
            {
                string json = _serializer.Serialize(_userData);
                _fileHandler.Write(FileName, json);
                Debug.Log($"[UserDataManager] Saved to {FileName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[UserDataManager] Failed to save data: {e}");
            }
        }
    }
}