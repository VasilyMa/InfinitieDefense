using UnityEngine;

namespace Client
{
    struct OreComponent
    {
        public int MaxAmount;
        public int CurrentAmount;
        public bool IsEnable;
        public float respawnTime;
        public float currentRespawnTime;
        public GameObject prefab;
        public GameObject[] OreParts;
        public GameObject cursor;
    }
}