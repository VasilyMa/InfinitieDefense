using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerStorage", menuName = "Configs/PlayerStorage", order = 0)]
public class PlayerStorage : ScriptableObject
{
    public Dictionary<string, PlayerData> Players;
    public GameObject[] PlayerPrefabs;

    public void Init()
    {
        Players = new Dictionary<string, PlayerData>
        {
            ["1level"] = new PlayerData
            {
                LVL = 1,
                Damage = 5,
                Speed = 3f,
                Health = 100,
                PlayerPrefab = PlayerPrefabs[0]
            },
            ["2level"] = new PlayerData
            {
                LVL = 2,
                Damage = 7,
                Speed = 3.5f,
                Health = 120,
                PlayerPrefab = PlayerPrefabs[1]
            },
        };
    }
    
    public int GetLevelByID(string id)
    {
        return Players[id].LVL;
    }
    public int GetDamageByID(string id)
    {
        return Players[id].Damage;
    }
    public float GetSpeedByID(string id)
    {
        return Players[id].Speed;
    }
    public int GetHealthByID(string id)
    {
        return Players[id].Health;
    }
    public GameObject GetPlayerByID(string id)
    {
        return Players[id].PlayerPrefab;
    }
}