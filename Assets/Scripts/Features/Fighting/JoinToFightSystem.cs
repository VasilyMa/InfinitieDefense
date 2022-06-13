using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class JoinToFightSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable, FightingComponent, ViewComponent>> _enemyFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<FightingComponent> _fightingPool = default;
        readonly EcsPoolInject<NotMovable> _notMovablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var fightingComponent = ref _fightingPool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                bool isNotMovable = _notMovablePool.Value.Has(entity);

                if (!isNotMovable && targetableComponent.DistanceToTarget < fightingComponent.ReachZone)
                {
                    _notMovablePool.Value.Add(entity);
                    viewComponent.Rigidbody.velocity = Vector3.zero;
                }
                else if(isNotMovable && targetableComponent.DistanceToTarget > fightingComponent.ReachZone)
                {
                    _notMovablePool.Value.Del(entity);
                }
            }
        }
    }
}