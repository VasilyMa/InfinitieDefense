using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStorage : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        [Header("Указываем сколько кораблей будет на Энкаунтере")]
        [Tooltip("Количество энкаунтеров")]
        public int[] Encounters;
        [Header("Задаем ближников на каждом из кораблей")]
        [Tooltip("Количество кораблей")]
        public int[] EnemyInShip;
        [Header("Задаем дальников на каждом из кораблей")]
        [Tooltip("Количество кораблей")]
        public int[] RangeEnemyInShip;
    }
    public List<Test> Waves = new List<Test>();
    public int GetAllEnemies()
    {
        int allEnemies = 0;
        for (int i = 0; i < Waves.Count; i++)
        {
            allEnemies += Waves[i].EnemyInShip.Length + Waves[i].RangeEnemyInShip.Length;
        }
        return allEnemies;
    }
}
