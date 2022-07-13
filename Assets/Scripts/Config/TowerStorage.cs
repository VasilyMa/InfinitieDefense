using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TowerStorage", menuName = "Configs/TowerStorage", order = 0)]
public class TowerStorage : ScriptableObject
{
    [Header("Виды замков")]
    public GameObject[] TowerPrefabs;
    [Header("Меши замков")]
    public Mesh[] TowerMeshs;
    public GameObject DefenderPrefab;
    public Dictionary<string, Tower> Towers;

    public void Init()
    {
        Towers = new Dictionary<string, Tower>
        {
            //MELEE
            ["1tower"] = new Tower
            {
                TowerLevel = 1,
                Radius = 25,
                TowerHealth = 2500,
                TowerPrefab = TowerPrefabs[0],
                TowerMesh = TowerMeshs[0],
                Upgrade = 1,
                IsLast = false,
                NextID = "2tower",
                DefenderCount = 0
            },
            ["2tower"] = new Tower
            {
                TowerLevel = 2,
                Radius = 40,
                TowerHealth = 2900,
                TowerPrefab = TowerPrefabs[0],
                TowerMesh = TowerMeshs[0],
                Upgrade = 2,
                IsLast = false,
                NextID = "3tower",
                DefenderCount = 1
            },
            ["3tower"] = new Tower
            {
                TowerLevel = 3,
                Radius = 50,
                TowerHealth = 3300,
                TowerPrefab = TowerPrefabs[0],
                TowerMesh = TowerMeshs[0],
                Upgrade = 3,
                IsLast = false,
                NextID = "4tower",
                DefenderCount = 2
            },
            ["4tower"] = new Tower
            {
                TowerLevel = 4,
                Radius = 60,
                TowerHealth = 3600,
                TowerPrefab = TowerPrefabs[1],
                TowerMesh = TowerMeshs[1],
                Upgrade = 4,
                NextID = "5tower",
                IsLast = false,
                DefenderCount = 3
            },
            ["5tower"] = new Tower
            {
                TowerLevel = 5,
                Radius = 65,
                TowerHealth = 3900,
                TowerPrefab = TowerPrefabs[1],
                TowerMesh = TowerMeshs[1],
                Upgrade = 5,
                NextID = "6tower",
                IsLast = false,
                DefenderCount = 4
            },
            ["6tower"] = new Tower
            {
                TowerLevel = 6,
                Radius = 70,
                TowerHealth = 4200,
                TowerPrefab = TowerPrefabs[1],
                TowerMesh = TowerMeshs[1],
                Upgrade = 6,
                NextID = "7tower",
                IsLast = false,
                DefenderCount = 5
            },
            ["7tower"] = new Tower
            {
                TowerLevel = 7,
                Radius = 75,
                TowerHealth = 4500,
                TowerPrefab = TowerPrefabs[2],
                TowerMesh = TowerMeshs[2],
                Upgrade = 7,
                NextID = "8tower",
                IsLast = false,
                DefenderCount = 6
            },
            ["8tower"] = new Tower
            {
                TowerLevel = 8,
                Radius = 80,
                TowerHealth = 4800,
                TowerPrefab = TowerPrefabs[2],
                TowerMesh = TowerMeshs[2],
                Upgrade = 8,
                NextID = "9tower",
                IsLast = false,
                DefenderCount = 7
            },
            ["9tower"] = new Tower
            {
                TowerLevel = 9,
                Radius = 85,
                TowerHealth = 5100,
                TowerPrefab = TowerPrefabs[2],
                TowerMesh = TowerMeshs[2],
                Upgrade = 999999,
                IsLast = true,
                DefenderCount = 8
            }
        };
    }
    public int GetRadiusByID(string id)
    {
        return Towers[id].Radius;
    }
    public int GetHealthByID(string id)
    {
        return Towers[id].TowerHealth;
    }
    public GameObject GetTowerPrefabByID(string id)
    {
        return Towers[id].TowerPrefab;
    }
    public Mesh GetTowerMeshByID(string id)
    {
        return Towers[id].TowerMesh;
    }
    public int GetUpgradeByID(string id)
    {
        return Towers[id].Upgrade;
    }
    public bool GetIsLastByID(string id)
    {
        return Towers[id].IsLast;
    }
    public string GetNextIDByID(string id)
    {
        return Towers[id].NextID;
    }
    public int GetDefenderCountByID(string id)
    {
        return Towers[id].DefenderCount;
    }
    public int GetLevelByID(string id)
    {
        return Towers[id].TowerLevel;
    }
}
