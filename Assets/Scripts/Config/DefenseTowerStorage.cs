using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefenseTowerStorage", menuName = "Configs/DefenseTowerStorage", order = 0)]
public class DefenseTowerStorage : ScriptableObject
{
    [Header("Виды башен")]
    public GameObject[] TowerPrefabs;
    [Header("Виды ядер для пушек")]
    public GameObject[] CannonBallPrefabs;
    public Dictionary<string, DefenseTower> Towers;

    public void Init()
    {
        Towers = new Dictionary<string, DefenseTower>
        {
            ["empty"] = new DefenseTower
            {
                NextID = "1tower",
                Upgrade = 1
            },
            ["1tower"] = new DefenseTower
            {
                Radius = 20,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 500,
                TowerPrefab = TowerPrefabs[0],
                CannonBallPrefab = CannonBallPrefabs[0],
                Upgrade = 5,
                IsLast = false,
                NextID = "2tower"
            },
            ["2tower"] = new DefenseTower
            {
                Radius = 7,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[1],
                CannonBallPrefab = CannonBallPrefabs[1],
                Upgrade = 7,
                IsLast = false,
                NextID = "3tower"
            },
            ["3tower"] = new DefenseTower
            {
                Radius = 9,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[2],
                CannonBallPrefab = CannonBallPrefabs[2],
                Upgrade = 7,
                IsLast = false,
                NextID = "4tower"
            },
            ["4tower"] = new DefenseTower
            {
                Radius = 11,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[3],
                CannonBallPrefab = CannonBallPrefabs[3],
                Upgrade = 13,
                IsLast = false,
                NextID = "5tower"
            },
            ["5tower"] = new DefenseTower
            {
                Radius = 13,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[4],
                CannonBallPrefab = CannonBallPrefabs[4],
                Upgrade = 15,
                IsLast = false,
                NextID = "6tower"
            },
            ["6tower"] = new DefenseTower
            {
                Radius = 15,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[5],
                CannonBallPrefab = CannonBallPrefabs[5],
                Upgrade = 17,
                IsLast = false,
                NextID = "7tower"
            },
            ["7tower"] = new DefenseTower
            {
                Radius = 17,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[6],
                CannonBallPrefab = CannonBallPrefabs[6],
                Upgrade = 20,
                IsLast = false,
                NextID = "8tower"
            },
            ["8tower"] = new DefenseTower
            {
                Radius = 19,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[7],
                CannonBallPrefab = CannonBallPrefabs[7],
                Upgrade = 23,
                IsLast = false,
                NextID = "9tower"
            },
            ["9tower"] = new DefenseTower
            {
                Radius = 21,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[8],
                CannonBallPrefab = CannonBallPrefabs[8],
                Upgrade = 25,
                IsLast = false,
                NextID = "10tower"
            },
            ["10tower"] = new DefenseTower
            {
                Radius = 23,
                Damage = 10,
                Cooldown = 2f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[9],
                CannonBallPrefab = CannonBallPrefabs[9],
                Upgrade = 30,
                IsLast = true
            },
        };
    }
    public int GetRadiusByID(string id)
    {
        return Towers[id].Radius;
    }
    public int GetDamageByID(string id)
    {
        return Towers[id].Damage;
    }
    public float GetCooldownByID(string id)
    {
        return Towers[id].Cooldown;
    }
    public int GetHealthByID(string id)
    {
        return Towers[id].TowerHealth;
    }
    public GameObject GetTowerPrefabByID(string id)
    {
        return Towers[id].TowerPrefab;
    }
    public GameObject GetCannonBallPrefabByID(string id)
    {
        return Towers[id].CannonBallPrefab;
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
