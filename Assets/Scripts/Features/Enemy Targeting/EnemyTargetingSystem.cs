using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemyTargetingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EnemyTag, Targetable>> _enemyFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        readonly EcsSharedInject<GameState> _gameState;

        public void Run (EcsSystems systems)
        {
            foreach(var enemyEntity in _enemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(enemyEntity);

                if (targetableComponent.TargetObject)
                {
                    continue;
                }

                ref var _viewMainTowerComponent = ref _viewPool.Value.Get(_gameState.Value.EntityMainTower);

                targetableComponent.TargetObject = _viewMainTowerComponent.GameObject;
            }
        }
    }
}