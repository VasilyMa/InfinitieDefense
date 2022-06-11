using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitMainTower : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        string MainTower;

        public void Init (EcsSystems systems)
        {
            var allMainTowers = GameObject.FindGameObjectsWithTag(nameof(MainTower));

            var world = systems.GetWorld();

            foreach (var mainTower in allMainTowers)
            {
                var mainTowerEntity = world.NewEntity();

                _gameState.Value.EntityMainTower = mainTowerEntity;

                world.GetPool<MainTowerTag>().Add(mainTowerEntity);

                ref var viewComponent = ref world.GetPool<ViewComponent>().Add(mainTowerEntity);
                viewComponent.GameObject = mainTower;
            }
        }
    }
}