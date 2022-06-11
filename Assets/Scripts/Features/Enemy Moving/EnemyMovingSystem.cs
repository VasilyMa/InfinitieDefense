using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemyMovingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EnemyTag, ViewComponent, Movable, Targetable>> _allEnemyFilter = default;

        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var enemyEntity in _allEnemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(enemyEntity);

                if (!targetableComponent.TargetObject)
                {
                    continue;
                }

                ref var movableComponent = ref _movablePool.Value.Get(enemyEntity);
                ref var viewComponent = ref _viewPool.Value.Get(enemyEntity);
                viewComponent.Rigidbody.velocity = (targetableComponent.TargetObject.transform.position - viewComponent.GameObject.transform.position).normalized * movableComponent.Speed;
            }
        }
    }
}