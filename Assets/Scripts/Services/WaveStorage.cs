using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStorage : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        [Header("Encounters count")]
        [Tooltip("Ship counts in")]
        public int[] Encounters;
        [Header("Ship counts")]
        [Tooltip("Melee on ship")]
        public int[] MeleeEnemyInShip;
        [Header("Ship counts")]
        [Tooltip("Range on ship")]
        public int[] RangeEnemyInShip;
    }
    public List<Test> Waves = new List<Test>();
    public int GetAllEnemies()
    {
        int allEnemies = 0;
        for (int i = 0; i < Waves.Count; i++)
        {
            int shipCount = 0;
            foreach (var encounter in Waves[i].Encounters)
            {
                shipCount += encounter;
            }

            for (int j = 0; j < shipCount; j++)
            {
                allEnemies += Waves[i].MeleeEnemyInShip[j] 
                + Waves[i].RangeEnemyInShip[j];
            }
        }
        return allEnemies;
    }
}
