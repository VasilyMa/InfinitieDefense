using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStorage : MonoBehaviour
{
    [System.Serializable]
    public class Test
    {
        [Header("��������� ������� �������� ����� �� ����������")]
        [Tooltip("���������� �����������")]
        public int[] Encounters;
        [Header("������ ��������� �� ������ �� ��������")]
        [Tooltip("���������� ��������")]
        public int[] MeleeEnemyInShip;
        [Header("������ ��������� �� ������ �� ��������")]
        [Tooltip("���������� ��������")]
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
