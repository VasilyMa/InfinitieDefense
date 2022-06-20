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
                Radius = 20,
                TowerHealth = 50,
                TowerPrefab = TowerPrefabs[0],
                Upgrade = 3,
                IsLast = false,
                NextID = "2tower"
            },
            ["2tower"] = new DefenseTower
            {
                Radius = 7,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[1],
                Upgrade = 5,
                IsLast = false,
                NextID = "3tower"
            },
            ["3tower"] = new DefenseTower
            {
                Radius = 9,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[2],
                Upgrade = 5,
                IsLast = false,
                NextID = "4tower"
            },
            ["4tower"] = new DefenseTower
            {
                Radius = 11,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[3],
                Upgrade = 5,
                IsLast = false,
                NextID = "5tower"
            },
            ["5tower"] = new DefenseTower
            {
                Radius = 13,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[4],
                Upgrade = 5,
                IsLast = false,
                NextID = "6tower"
            },
            ["6tower"] = new DefenseTower
            {
                Radius = 15,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[5],
                Upgrade = 5,
                IsLast = false,
                NextID = "7tower"
            },
            ["7tower"] = new DefenseTower
            {
                Radius = 17,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[6],
                Upgrade = 5,
                IsLast = false,
                NextID = "8tower"
            },
            ["8tower"] = new DefenseTower
            {
                Radius = 19,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[7],
                Upgrade = 5,
                IsLast = false,
                NextID = "9tower"
            },
            ["9tower"] = new DefenseTower
            {
                Radius = 21,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[8],
                Upgrade = 5,
                IsLast = false,
                NextID = "10tower"
            },
            ["10tower"] = new DefenseTower
            {
                Radius = 23,
                TowerHealth = 70,
                TowerPrefab = TowerPrefabs[9],
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
