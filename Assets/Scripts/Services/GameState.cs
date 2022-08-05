using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using System;
using UnityEngine.SceneManagement;

namespace Client
{
    public class GameState
    {
        public Saves Saves = new Saves();
        public EcsWorld World;
        public int EntityInterface;
        public int EntityPlayer;
        public int EntityMainTower;
        public int EntityCamera;
        public int EntityPlayerUpgradePoint;

        public TowerStorage TowerStorage;
        public InterfaceStorage InterfaceStorage;
        public DropableItemStorage DropableItemStorage;
        public PlayerStorage PlayerStorage;
        public DefenseTowerStorage DefenseTowerStorage;
        public WaveStorage WaveStorage;
        public EnemyConfig EnemyConfig;
        public ExplosionStorage ExplosionStorage;
        public LevelsStorage LevelsStorage;

        public int RockCount = 0;
        public int CoinCount = 0;
        public int CurrentEncounter = 0;
        private int CurrentWave = -1;
        private int _currentUpgradeTower = 0;
        public List<Transform> CoinTransformList = new List<Transform>();
        public List<Transform> StoneTransformList = new List<Transform>();
        public GameObject[] DefendersGOs;
        public int[] DefendersEntity;
        public string[] DefenseTowers;
        public int[] TowersUpgrade;
        public int[] TowersEntity;
        public int TowerCount;
        public int TowersInRow;
        public float TimeToNextWave;
        public string CurrentPlayerID;
        public int PlayerExperience;
        public int AllEnemies;
        public int EnemiesWave;
        public int StaticEnemiesWave;
        public bool isWave;
        public int MainTowerUpgrade;
        public float KillsCount;
        public float DelayBeforUpgrade = 0.8f;
        public float DelayAfterUpgrade = 0.5f;
        public Biom CurrentBiom;

        public GameState(EcsWorld world, TowerStorage towerStorage, InterfaceStorage interfaceStorage, DropableItemStorage dropableItemStorage,
        PlayerStorage playerStorage, DefenseTowerStorage defenseTowerStorage, int towerCount, int towersInRow, float timeToNextWave, WaveStorage waveStorage,
        EnemyConfig enemyConfig, ExplosionStorage explosionStorage, LevelsStorage levelConfig)
        {
            World = world;
            TowerStorage = towerStorage;
            InterfaceStorage = interfaceStorage;
            DropableItemStorage = dropableItemStorage;
            PlayerStorage = playerStorage;
            DefenseTowerStorage = defenseTowerStorage;
            TowerCount = towerCount;
            TowersInRow = towersInRow;
            TimeToNextWave = timeToNextWave;
            WaveStorage = waveStorage;
            EnemyConfig = enemyConfig;
            ExplosionStorage = explosionStorage;
            LevelsStorage = levelConfig;
            PlayerStorage.Init();
            TowerStorage.Init();
            DefenseTowerStorage.Init();
            Saves.InitSave();
            CurrentPlayerID = Saves.PlayerID;
            CoinCount = Saves.Coin;
            PlayerExperience = Saves.PlayerUpgrade;
            isWave = true;
            //InitSaves();
            InitDefenseTowers();    
            InitDefenders();
            InitBiom();
            TowersUpgrade[0] = Saves.MainTowerUpgrade;
            KillsCount = 0;
        }
        public void InitDefenders()
        {
            DefendersEntity = new int[10];
            DefendersGOs = new GameObject[10];
        }
        /*private void InitSaves()
        {
            Saves.InitSave();
            CurrentPlayerID = Saves.PlayerID;
            PlayerLevel = 0;
            CurrentWave = Saves.CurrentWave;
            CoinCount = Saves.Coin;
            RockCount = Saves.Rock;
            Debug.Log("Save " + Saves.Rock + "State " + RockCount);
        }*/

        public void InitDefenseTowers()
        {
            DefenseTowers = new string[TowerCount];
            TowersUpgrade = new int[TowerCount];
            TowersEntity = new int[TowerCount];
            for (int i = 0; i < DefenseTowers.Length;i++)
            {
                /*DefenseTowers[i] = Saves.TowerID[i];
                TowersUpgrade[i] = Saves.TowersUpgrade[i];*/
                if (i == 0)
                {
                    DefenseTowers[i] = "1tower";
                }
                else
                {
                    DefenseTowers[i] = "empty";
                }
                TowersUpgrade[i] = 0;
            }

        }
        public bool PointInMainTowerRadius(Vector3 position)
        {
            int radius = TowerStorage.GetRadiusByID(DefenseTowers[0]);
            if((Mathf.Pow(position.x,2))+ (Mathf.Pow(position.z,2)) <= Mathf.Pow(radius,2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpgradeTower(int towerIndex)
        {
            TowersUpgrade[towerIndex]++;
            int neededUpgradeValue = 0;
            if(towerIndex == 0)
            {
                neededUpgradeValue = TowerStorage.GetUpgradeByID(DefenseTowers[towerIndex]);
                Saves.SaveMainTowerUpgrade(TowersUpgrade[towerIndex]);
            }
            else
            {
                neededUpgradeValue = DefenseTowerStorage.GetUpgradeByID(DefenseTowers[towerIndex]);
            }

            if(TowersUpgrade[towerIndex] == neededUpgradeValue)
            {
                TowersUpgrade[towerIndex] = 0;
                Saves.SaveMainTowerUpgrade(TowersUpgrade[0]);
                ref var createNextTowerComp = ref World.GetPool<CreateNextTowerEvent>().Add(TowersEntity[towerIndex]);
                createNextTowerComp.TowerIndex = towerIndex;
                createNextTowerComp.Change = true;
            }
        }
        public void UpgradePlayer()
        {
            PlayerExperience++;
            int neededUpgradeValue = PlayerStorage.GetUpgradeByID(CurrentPlayerID);
            if(PlayerExperience == neededUpgradeValue)
            {
                PlayerExperience = 0;
                World.GetPool<CreateNewPlayerEvent>().Add(EntityPlayer);
            }
        }

        public void SetNextEncounter()
        {
            CurrentEncounter++;
        }

        public int GetCurrentEncounter()
        {
            return CurrentEncounter;
        }

        public void SetNextWave()
        {
            EcsFilter inactiveShipsFilter = World.Filter<ShipTag>().Inc<InactiveTag>().Inc<CurrentWaveTag>().End();
            foreach (var inactiveShip in inactiveShipsFilter)
            {
                ref var shipComponent = ref World.GetPool<ShipComponent>().Get(inactiveShip);
                if (shipComponent.Encounter >= GetCurrentEncounter())
                {
                    World.GetPool<InactiveTag>().Del(inactiveShip);
                }
            }

            CurrentWave++;
            World.GetPool<ActivateWaveShipsEvent>().Add(World.NewEntity());
            CurrentEncounter = 0;
            CalculateAllEnemies();
        }

        public int GetCurrentWave()
        {
            return CurrentWave;
        }
        private void CalculateAllEnemies()
        {
            EnemiesWave = 0;
            for (int i = 0; i < WaveStorage.Waves[CurrentWave].Encounters.Length; i++)
            {
                for (int y = 0; y < WaveStorage.Waves[CurrentWave].Encounters[i]; y++)
                {
                    for (int x = 0; x < WaveStorage.Waves[CurrentWave].MeleeEnemyInShip[y]; x++)
                    {
                        EnemiesWave++;
                    }
                    for (int z = 0; z < WaveStorage.Waves[CurrentWave].RangeEnemyInShip[y]; z++)
                    {
                        EnemiesWave++;
                    }
                }
            }
            StaticEnemiesWave = EnemiesWave;
            Debug.Log($"Вражеские пидоры {EnemiesWave}");
        }
        public void InitBiom()
        {
            for (int b = 0; b < LevelsStorage.StartBiomLevels.Length; b++)
            {
                var biom = new Biom();
                biom.StartBiomLevel = LevelsStorage.StartBiomLevels[b];
                biom.BiomType = LevelsStorage.StartBiomTypes[b];
                biom.BiomSprite = LevelsStorage.BiomsSprites[b];
                if (b + 1 < LevelsStorage.StartBiomLevels.Length)
                {
                    biom.NextBiomSprite = LevelsStorage.BiomsSprites[b + 1];
                }
                else
                {
                    biom.NextBiomSprite = LevelsStorage.BiomsSprites[0];
                }
                if (b == 0 || (b > 0 && b < LevelsStorage.StartBiomLevels.Length - 1))
                {
                    int length = LevelsStorage.StartBiomLevels[b + 1] - LevelsStorage.StartBiomLevels[b];
                    biom.BiomLevels = new List<int>();
                    for (int i = 0; i < length; i++) biom.BiomLevels.Add(LevelsStorage.StartBiomLevels[b] + i);
                }
                else if (b == LevelsStorage.StartBiomLevels.Length - 1)
                {
                    int length = (SceneManager.sceneCountInBuildSettings - 1) - (LevelsStorage.StartBiomLevels[b] - 1);
                    biom.BiomLevels = new List<int>();
                    for (int i = 0; i < length; i++) biom.BiomLevels.Add(LevelsStorage.StartBiomLevels[b] + i);
                }

                LevelsStorage.Bioms = new List<Biom>();
                LevelsStorage.Bioms.Add(biom);

                if (biom.BiomLevels.Contains(SceneManager.GetActiveScene().buildIndex))
                    CurrentBiom = biom;
            }
        }
    }
}
