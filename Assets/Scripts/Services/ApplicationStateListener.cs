using System;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ApplicationStateListener : MonoBehaviour
    {
        [Inject]
        private UserDataManager _userDataManager;
        
        private void OnApplicationQuit() => _userDataManager.Save();

        private void OnApplicationPause(bool pauseStatus) => _userDataManager.Save();
    }
}