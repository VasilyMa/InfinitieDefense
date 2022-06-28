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
        public int[] EnemyInShip;
        [Header("������ ��������� �� ������ �� ��������")]
        [Tooltip("���������� ��������")]
        public int[] RangeEnemyInShip;
    }
    public List<Test> Waves = new List<Test>();
    
}
