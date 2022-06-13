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

        public GameState(EcsWorld world, TowerStorage towerStorage, InterfaceStorage interfaceStorage)
        {
            World = world;
            TowerStorage = towerStorage;
            InterfaceStorage = interfaceStorage;
            TowerStorage.Init();
        }
    }
}
