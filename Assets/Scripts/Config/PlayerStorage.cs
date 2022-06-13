using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerStorage", menuName = "Configs/PlayerStorage", order = 0)]
public class PlayerStorage : ScriptableObject
{
    public Dictionary<string, Player> Players;
    public GameObject[] PlayerPrefabs;

    public void Init()
    {
        Players = new Dictionary<string, Player>
        {
            ["1level"] = new Player
            {
                LVL = 1,
                Damage = 5,
                Speed = 3f,
                Health = 100,
                PlayerPrefab = PlayerPrefabs[0]
            },
            ["2level"] = new Player
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
