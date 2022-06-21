using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TowerStorage", menuName = "Configs/TowerStorage", order = 0)]
public class TowerStorage : ScriptableObject
{
    public GameObject[] TowerPrefabs;
    public GameObject DefenderPrefab;
    public Dictionary<string, Tower> Towers;

    public void Init()
    {
        Towers = new Dictionary<string, Tower>
        {
            //MELEE
            ["1tower"] = new Tower
            {
                Radius = 15,
                TowerHealth = 50,
                TowerPrefab = TowerPrefabs[0],
                Upgrade = 3,
                IsLast = false,
                NextID = "2tower",
                DefenderCount = 3
            },
            ["2tower"] = new Tower
            {
                Radius = 17,
                TowerHealth = 120,
                TowerPrefab = TowerPrefabs[1],
                Upgrade = 5,
                NextID = "3tower",
                IsLast = false,
                DefenderCount = 6
            },
            ["3tower"] = new Tower
            {
                Radius = 20,
                TowerHealth = 140,
                TowerPrefab = TowerPrefabs[2],
                Upgrade = 7,
                IsLast = true,
                DefenderCount = 9
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
    
}
