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
        public int EntityCamera;
        public TowerStorage TowerStorage;
        public InterfaceStorage InterfaceStorage;
        public PlayerStorage PlayerStorage;
        public DefenseTowerStorage DefenseTowerStorage;
        public WaveStorage WaveStorage;
        public EnemyConfig EnemyConfig;
        public int RockCount = 0;
        public int CoinCount = 0;
        public int CurrentEncounter = 0;
        private int CurrentWave = 0;
        private int _currentUpgradeTower = 0;
        public List<Transform> CoinTransformList = new List<Transform>();
        public List<Transform> StoneTransformList = new List<Transform>();
        public GameObject[] DefendersGOs;
        public int[] DefendersEntity;
        public string[] DefenseTowers;
        public int[] TowersUpgrade;
        public int[] TowersEntity;
        public int TowerCount;
        public string CurrentPlayerID;
        public int PlayerUpgrade;
        public int AllEnemies;
        public GameState(EcsWorld world, TowerStorage towerStorage, InterfaceStorage interfaceStorage, 
        PlayerStorage playerStorage, DefenseTowerStorage defenseTowerStorage, int towerCount, WaveStorage waveStorage,
        EnemyConfig enemyConfig)
        {
            World = world;
            TowerStorage = towerStorage;
            InterfaceStorage = interfaceStorage;
            PlayerStorage = playerStorage;
            DefenseTowerStorage = defenseTowerStorage;
            TowerCount = towerCount;
            WaveStorage = waveStorage;
            EnemyConfig = enemyConfig;
            PlayerStorage.Init();
            TowerStorage.Init();
            DefenseTowerStorage.Init();
            
            
            InitSaves();
            InitDefenseTowers();
            InitDefenders();
        }
        public void InitDefenders()
        {
            DefendersEntity = new int[10];
            DefendersGOs = new GameObject[10];
        }
        private void InitSaves()
        {
            Saves.InitSave();
            CurrentPlayerID = Saves.PlayerID;
            PlayerUpgrade = 0;
            CurrentWave = Saves.CurrentWave;
            CoinCount = Saves.Coin;
            RockCount = Saves.Rock;
            Debug.Log("Save " + Saves.Rock + "State " + RockCount);
        }

        public void InitDefenseTowers()
        {
            DefenseTowers = new string[TowerCount];
            TowersUpgrade = new int[TowerCount];
            TowersEntity = new int[TowerCount];
            for (int i = 0; i < DefenseTowers.Length;i++)
            {
                DefenseTowers[i] = Saves.TowerID[i];
                TowersUpgrade[i] = Saves.TowersUpgrade[i];
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
            }
            else
            {
                neededUpgradeValue = DefenseTowerStorage.GetUpgradeByID(DefenseTowers[towerIndex]);
            }

            if(TowersUpgrade[towerIndex] == neededUpgradeValue)
            {
                TowersUpgrade[towerIndex] = 0;
                ref var createNextTowerComp = ref World.GetPool<CreateNextTowerEvent>().Add(TowersEntity[towerIndex]);
                createNextTowerComp.TowerIndex = towerIndex;
                createNextTowerComp.Change = true;
            }
        }
        public void UpgradePlayer()
        {
            PlayerUpgrade++;
            int neededUpgradeValue = PlayerStorage.GetUpgradeByID(CurrentPlayerID);
            if(PlayerUpgrade == neededUpgradeValue)
            {
                PlayerUpgrade = 0;
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
        }

        public int GetCurrentWave()
        {
            return CurrentWave;
        }


    }
}
