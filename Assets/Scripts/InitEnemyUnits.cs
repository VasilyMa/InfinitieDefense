using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitEnemyUnits : IEcsInitSystem
    {

        EcsSharedInject<GameState> _gameState = default;

        string Enemy;
        public void Init(EcsSystems systems)
        {
            var allEnemyUnits = GameObject.FindGameObjectsWithTag(nameof(Enemy));

            var world = systems.GetWorld();

            foreach (var enemy in allEnemyUnits)
            {
                var enemyEntity = world.NewEntity();

                world.GetPool<EnemyTag>().Add(enemyEntity);

                world.GetPool<Targetable>().Add(enemyEntity);

                ref var movableComponent = ref world.GetPool<Movable>().Add(enemyEntity);
                movableComponent.Speed = 5f;

                ref var viewComponent = ref world.GetPool<ViewComponent>().Add(enemyEntity);
                viewComponent.GameObject = enemy;
                viewComponent.Rigidbody = enemy.GetComponent<Rigidbody>();
            }
        }
    }
}