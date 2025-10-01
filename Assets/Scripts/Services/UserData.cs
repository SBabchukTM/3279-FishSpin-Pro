using System;

namespace Services
{
    [Serializable]
    public class UserData
    {
        public UserFinishedTutorialData UserFinishedTutorialData = new ();
        public GameSettingsData GameSettingsData = new();
        public UserLibraryData UserLibraryData = new();
        public UserRecordsData UserRecordsData = new();
    }
}