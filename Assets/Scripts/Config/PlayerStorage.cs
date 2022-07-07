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
                Damage = 10,
                Speed = 10f,
                Health = 100,
                PlayerPrefab = PlayerPrefabs[0],
                IsLast = false,
                NextID = "2level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[0]
            },
            ["2level"] = new PlayerData
            {
                LVL = 2,
                Damage = 20,
                Speed = 30f,
                Health = 70,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "3level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["3level"] = new PlayerData
            {
                LVL = 3,
                Damage = 30,
                Speed = 12f,
                Health = 90,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "4level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["4level"] = new PlayerData
            {
                LVL = 4,
                Damage = 40,
                Speed = 13f,
                Health = 110,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "5level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["5level"] = new PlayerData
            {
                LVL = 5,
                Damage = 50,
                Speed = 14f,
                Health = 130,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "6level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["6level"] = new PlayerData
            {
                LVL = 6,
                Damage = 60,
                Speed = 15f,
                Health = 150,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "7level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["7level"] = new PlayerData
            {
                LVL = 7,
                Damage = 70,
                Speed = 16f,
                Health = 170,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "8level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["8level"] = new PlayerData
            {
                LVL = 8,
                Damage = 80,
                Speed = 17f,
                Health = 190,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = false,
                NextID = "9level",
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
            },
            ["9level"] = new PlayerData
            {
                LVL = 9,
                Damage = 90,
                Speed = 18f,
                Health = 210,
                PlayerPrefab = PlayerPrefabs[1],
                IsLast = true,
                UpgradeValue = 1,
                Mesh = PlayerMeshes[1]
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
