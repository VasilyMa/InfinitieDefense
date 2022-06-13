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

        public GameState(EcsWorld world, InterfaceConfig _interfaceConfig)
        {
            World = world;
            InterfaceConfig = _interfaceConfig;
        }
    }
}
