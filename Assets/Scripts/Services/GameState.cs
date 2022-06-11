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

        public GameState(EcsWorld world)
        {
            World = world;
        }
    }
}
