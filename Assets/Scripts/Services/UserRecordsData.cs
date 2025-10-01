using System;
using System.Collections.Generic;

namespace Services
{
    [Serializable]
    public class UserRecordsData
    {
        public List<UserFishRecord> Records = new List<UserFishRecord>();
    }

    [Serializable]
    public class UserFishRecord
    {
        public string Name;
        public string Location;
        
        public int FishId;
        
        public string Time;
        public string Weight;
        
        public int year;
        public int month;
        public int day;

        public UserFishRecord(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public DateTime ToDateTime() => new DateTime(year, month, day);

        public override bool Equals(object obj)
        {
            return obj is UserFishRecord other &&
                   year == other.year &&
                   month == other.month &&
                   day == other.day;
        }

        public override int GetHashCode() => ToDateTime().GetHashCode();
    }
}