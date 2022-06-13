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
        public int EntityMainTower;
        public TowerStorage TowerStorage;
        public InterfaceStorage InterfaceStorage;
        public PlayerStorage PlayerStorage;
        public string CurrentTowerID;

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
    }
}
