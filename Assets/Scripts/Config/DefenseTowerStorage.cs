using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
    [Header("Спрайты ресурсов")]
    public Sprite ImageResources;

    private float _commonCD = 1f;

    public void Init()
    {
        Towers = new Dictionary<string, DefenseTower>
        {
            ["empty"] = new DefenseTower
            {
                NextID = "1tower",
                Upgrade = 10,
                ImageResource = ImageResources
            },
            ["1tower"] = new DefenseTower
            {
                LevelTower = 1,
                Radius = 7,
                Damage = 15,
                Cooldown = _commonCD,
                TowerHealth = 200,
                TowerPrefab = TowerPrefabs[0],
                TowerMesh = TowerMeshs[0],
                CannonBallPrefab = CannonBallPrefabs[0],
                Upgrade = 20,
                IsLast = false,
                NextID = "2tower",
                ImageResource = ImageResources
            },
            ["2tower"] = new DefenseTower
            {
                LevelTower = 2,
                Radius = 9,
                Damage = 20,
                Cooldown = _commonCD,
                TowerHealth = 250,
                TowerPrefab = TowerPrefabs[1],
                TowerMesh = TowerMeshs[1],
                CannonBallPrefab = CannonBallPrefabs[1],
                Upgrade = 30,
                IsLast = false,
                NextID = "3tower",
                ImageResource = ImageResources
            },
            ["3tower"] = new DefenseTower
            {
                LevelTower = 3,
                Radius = 11,
                Damage = 25,
                Cooldown = _commonCD,
                TowerHealth = 300,
                TowerPrefab = TowerPrefabs[2],
                TowerMesh = TowerMeshs[2],
                CannonBallPrefab = CannonBallPrefabs[2],
                Upgrade = 40,
                IsLast = false,
                NextID = "4tower",
                ImageResource = ImageResources
            },
            ["4tower"] = new DefenseTower
            {
                LevelTower = 4,
                Radius = 13,
                Damage = 30,
                Cooldown = _commonCD,
                TowerHealth = 350,
                TowerPrefab = TowerPrefabs[3],
                TowerMesh = TowerMeshs[3],
                CannonBallPrefab = CannonBallPrefabs[3],
                Upgrade = 60,
                IsLast = false,
                NextID = "5tower",
                ImageResource = ImageResources
            },
            ["5tower"] = new DefenseTower
            {
                LevelTower = 5,
                Radius = 15,
                Damage = 50,
                Cooldown = _commonCD,
                TowerHealth = 400,
                TowerPrefab = TowerPrefabs[4],
                TowerMesh = TowerMeshs[4],
                CannonBallPrefab = CannonBallPrefabs[4],
                Upgrade = 99999,
                IsLast = true,
                ImageResource = ImageResources
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
    public int GetLevelByID(string id)
    {
        return Towers[id].LevelTower;
    }
    public Sprite GetImageByID(string id)
    {
        return Towers[id].ImageResource;
    }
}
