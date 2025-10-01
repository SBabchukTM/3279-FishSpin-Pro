using System;
using System.Collections.Generic;
using AssetsProvider;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "FishesConfig", menuName = "Configs/FishesConfig")]
    public class FishesConfig : BaseConfig
    {
        public List<FishData> Fishes = new List<FishData>();
    }

    [Serializable]
    public class FishData
    {
        public Sprite Sprite;
        public string Name;
        public string Temperature;
        public string FavFood;
        public string FunFact;
        public string BestTime;
        public Color Color = new Color(1, 1, 1, 1);
    }
}