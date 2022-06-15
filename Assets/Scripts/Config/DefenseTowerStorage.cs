using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefenseTowerStorage", menuName = "Configs/DefenseTowerStorage", order = 0)]
public class DefenseTowerStorage : ScriptableObject
{
    public GameObject[] TowerPrefabs;
    public Dictionary<string, DefenseTower> Towers;

    public void Init()
    {
        Towers = new Dictionary<string, DefenseTower>
        {
            ["empty"] = new DefenseTower
            {
                NextID = "1tower",
                Upgrade = 2
            },
            ["1tower"] = new DefenseTower
            {
                Radius = 10,
                TowerHealth = 50,
                TowerPrefab = TowerPrefabs[0],
                Upgrade = 3,
                IsLast = false,
                NextID = "2tower"
            },
            ["2tower"] = new DefenseTower
            {
                Radius = 12,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[1],
                Upgrade = 5,
                IsLast = true
            },
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
    
}
