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
        public InterfaceConfig InterfaceConfig;
        public int EntityMainTower;
        public int EntityInterface;
        public int EntityPlayer;
        public int EntityCamera;
        public TowerStorage TowerStorage;
        public InterfaceStorage InterfaceStorage;
        public PlayerStorage PlayerStorage;
        public string CurrentTowerID;
        public int RockCount = 0;
        public int CoinCount = 0;
        private int _currentUpgradeTower = 0;
        public List<Transform> CoinTransformList = new List<Transform>();
        public List<Transform> StoneTransformList = new List<Transform>();

        public GameState(EcsWorld world, TowerStorage towerStorage, InterfaceStorage interfaceStorage, PlayerStorage playerStorage)
        {
            World = world;
            TowerStorage = towerStorage;
            InterfaceStorage = interfaceStorage;
            PlayerStorage = playerStorage;
            PlayerStorage.Init();
            TowerStorage.Init();
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
        public void UpgradeTower()
        {
            _currentUpgradeTower++;
            if(_currentUpgradeTower == TowerStorage.GetUpgradeByID(CurrentTowerID))
            {
                _currentUpgradeTower = 0;
                World.GetPool<CreateNextTowerEvent>().Add(EntityMainTower);
            }
        }


    }
}
