using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerStorage", menuName = "Configs/PlayerStorage", order = 0)]
public class PlayerStorage : ScriptableObject
{
    public Dictionary<string, PlayerData> Players;
    public GameObject[] PlayerPrefabs;
    public Mesh[] PlayerMeshes;

    public void Init()
    {
        Players = new Dictionary<string, PlayerData>
        {
            ["1level"] = new PlayerData
            {
                LVL = 1,
                Damage = 5,
                Speed = 6f,
                Health = 50,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = false,
                NextID = "2level",
                UpgradeValue = 1, // to do ay up to 10
                Mesh = PlayerMeshes[0]
            },
            ["2level"] = new PlayerData
            {
                LVL = 2,
                Damage = 10,
                Speed = 6f,
                Health = 70,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = false,
                NextID = "3level",
                UpgradeValue = 20,
                Mesh = PlayerMeshes[1]
            },
            ["3level"] = new PlayerData
            {
                LVL = 3,
                Damage = 20,
                Speed = 6f,
                Health = 90,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = false,
                NextID = "4level",
                UpgradeValue = 40,
                Mesh = PlayerMeshes[2]
            },
            ["4level"] = new PlayerData
            {
                LVL = 4,
                Damage = 25,
                Speed = 6f,
                Health = 110,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = false,
                NextID = "5level",
                UpgradeValue = 80,
                Mesh = PlayerMeshes[3]
            },
            ["5level"] = new PlayerData
            {
                LVL = 5,
                Damage = 30,
                Speed = 6f,
                Health = 130,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = true,
                UpgradeValue = 99999,
                Mesh = PlayerMeshes[4]
            }
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
    public bool GetIsLastByID(string id)
    {
        return Players[id].IsLast;
    }
    public string GetNextIdByID(string id)
    {
        return Players[id].NextID;
    }
    public int GetUpgradeByID(string id)
    {
        return Players[id].UpgradeValue;
    }
    public Mesh GetMeshByID(string id)
    {
        return Players[id].Mesh;
    }
}
