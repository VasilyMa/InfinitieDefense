using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStorage : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        public int[] EnemyInShip;
        public int[] Encounters;//размер это колличество энкаунтеров, значение сколько активных врагов на энкаунтере
    }
    public List<Test> Waves = new List<Test>();
    
}
