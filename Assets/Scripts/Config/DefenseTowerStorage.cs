using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefenseTowerStorage", menuName = "Configs/DefenseTowerStorage", order = 0)]
public class DefenseTowerStorage : ScriptableObject
{
    [Header("Виды башен")]
    public GameObject[] TowerPrefabs;
    [Header("Меши Башен")]
    public Mesh[] TowerMeshs;
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
                Radius = 12,
                Damage = 1,
                Cooldown = 5f,
                TowerHealth = 450,
                TowerPrefab = TowerPrefabs[0],
                TowerMesh = TowerMeshs[0],
                CannonBallPrefab = CannonBallPrefabs[0],
                Upgrade = 2,
                IsLast = false,
                NextID = "2tower"
            },
            ["2tower"] = new DefenseTower
            {
                Radius = 14,
                Damage = 5,
                Cooldown = 5f,
                TowerHealth = 450,
                TowerPrefab = TowerPrefabs[1],
                TowerMesh = TowerMeshs[1],
                CannonBallPrefab = CannonBallPrefabs[1],
                Upgrade = 3,
                IsLast = false,
                NextID = "3tower"
            },
            ["3tower"] = new DefenseTower
            {
                Radius = 16,
                Damage = 5,
                Cooldown = 5f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[2],
                TowerMesh = TowerMeshs[2],
                CannonBallPrefab = CannonBallPrefabs[2],
                Upgrade = 4,
                IsLast = false,
                NextID = "4tower"
            },
            ["4tower"] = new DefenseTower
            {
                Radius = 18,
                Damage = 7,
                Cooldown = 4f,
                TowerHealth = 700,
                TowerPrefab = TowerPrefabs[3],
                TowerMesh = TowerMeshs[3],
                CannonBallPrefab = CannonBallPrefabs[3],
                Upgrade = 5,
                IsLast = false,
                NextID = "5tower"
            },
            ["5tower"] = new DefenseTower
            {
                Radius = 20,
                Damage = 7,
                Cooldown = 4f,
                TowerHealth = 1000,
                TowerPrefab = TowerPrefabs[4],
                TowerMesh = TowerMeshs[4],
                CannonBallPrefab = CannonBallPrefabs[4],
                Upgrade = 6,
                IsLast = false,
                NextID = "6tower"
            },
            ["6tower"] = new DefenseTower
            {
                Radius = 22,
                Damage = 10,
                Cooldown = 5f,
                TowerHealth = 1000,
                TowerPrefab = TowerPrefabs[5],
                TowerMesh = TowerMeshs[5],
                CannonBallPrefab = CannonBallPrefabs[5],
                Upgrade = 7,
                IsLast = false,
                NextID = "7tower"
            },
            ["7tower"] = new DefenseTower
            {
                Radius = 24,
                Damage = 10,
                Cooldown = 5f,
                TowerHealth = 1300,
                TowerPrefab = TowerPrefabs[6],
                TowerMesh = TowerMeshs[6],
                CannonBallPrefab = CannonBallPrefabs[6],
                Upgrade = 8,
                IsLast = false,
                NextID = "8tower"
            },
            ["8tower"] = new DefenseTower
            {
                Radius = 26,
                Damage = 10,
                Cooldown = 4f,
                TowerHealth = 1300,
                TowerPrefab = TowerPrefabs[7],
                TowerMesh = TowerMeshs[7],
                CannonBallPrefab = CannonBallPrefabs[7],
                Upgrade = 9,
                IsLast = false,
                NextID = "9tower"
            },
            ["9tower"] = new DefenseTower
            {
                Radius = 28,
                Damage = 10,
                Cooldown = 4f,
                TowerHealth = 1600,
                TowerPrefab = TowerPrefabs[8],
                TowerMesh = TowerMeshs[8],
                CannonBallPrefab = CannonBallPrefabs[8],
                Upgrade = 10,
                IsLast = false,
                NextID = "10tower"
            },
            ["10tower"] = new DefenseTower
            {
                Radius = 30,
                Damage = 20,
                Cooldown = 6f,
                TowerHealth = 1600,
                TowerPrefab = TowerPrefabs[9],
                TowerMesh = TowerMeshs[9],
                CannonBallPrefab = CannonBallPrefabs[9],
                Upgrade = 999999,
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
    public Mesh GetTowerMeshByID(string id)
    {
        return Towers[id].TowerMesh;
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
