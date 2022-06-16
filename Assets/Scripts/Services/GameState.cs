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
        public EcsWorld World;
<<<<<<< HEAD
        public InterfaceConfig InterfaceConfig;
=======
        public int EntityMainTower;
>>>>>>> origin/develop
        public int EntityInterface;
        public int EntityPlayer;
        public int EntityCamera;
        public TowerStorage TowerStorage;
        public InterfaceStorage InterfaceStorage;
        public PlayerStorage PlayerStorage;
        public DefenseTowerStorage DefenseTowerStorage;
        public string CurrentTowerID;
        public int RockCount = 0;
        public int CoinCount = 0;
        public int CurrentActivatedShip = 0;
        private int Wave = 0;
        private int _currentUpgradeTower = 0;
        public List<Transform> CoinTransformList = new List<Transform>();
        public List<Transform> StoneTransformList = new List<Transform>();
        public string[] DefenseTowers;
        public int[] TowersUpgrade;
        public int[] TowersEntity;
        public int TowerCount;

        public GameState(EcsWorld world, TowerStorage towerStorage, InterfaceStorage interfaceStorage, 
        PlayerStorage playerStorage, DefenseTowerStorage defenseTowerStorage, int towerCount)
        {
            World = world;
            TowerStorage = towerStorage;
            InterfaceStorage = interfaceStorage;
            PlayerStorage = playerStorage;
            DefenseTowerStorage = defenseTowerStorage;
            TowerCount = towerCount;
            PlayerStorage.Init();
            TowerStorage.Init();
            DefenseTowerStorage.Init();
            InitDefenseTowers();
        }

        public void InitDefenseTowers()
        {
            DefenseTowers = new string[TowerCount];
            TowersUpgrade = new int[TowerCount];
            TowersEntity = new int[TowerCount];
            for (int i = 0; i < DefenseTowers.Length;i++)
            {
                if(i == 0) DefenseTowers[i] = "1tower";
                else DefenseTowers[i] = "empty";

                TowersUpgrade[i] = 0;
            }

        }
        public bool PointInMainTowerRadius(Vector3 position)
        {
            int radius = TowerStorage.GetRadiusByID(CurrentTowerID);
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
            }
        }

        public void SetNextWave()
        {
            World.GetPool<ActivateWaveShipsEvent>().Add(World.NewEntity());
            Wave++;
            CurrentActivatedShip = 0;
        }

        public int GetCurrentWave()
        {
            return Wave;
        }


    }
}
