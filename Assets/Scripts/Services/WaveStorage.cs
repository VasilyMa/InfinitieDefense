using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStorage : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        public string name; // дополнительное поле, чтобы в инсекторе отобразить имя массива для удобства
        public int[] EnemyInShip;
    }
    public List<Test> Waves = new List<Test>();
    public int GetWavesCount()
    {
        return Waves.Count;
    }
    public int GetShipCountByIndex(int index)
    {
        return Waves[index].EnemyInShip.Length;
    }
}
