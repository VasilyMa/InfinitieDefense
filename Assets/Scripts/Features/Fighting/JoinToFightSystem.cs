using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class JoinToFightSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable, FightingComponent, ViewComponent>, Exc<InactiveTag>> _enemyFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<FightingComponent> _fightingPool = default;
        readonly EcsPoolInject<InFightTag> _inFightPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var fightingComponent = ref _fightingPool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                bool isNotMovable = _inFightPool.Value.Has(entity);

                if (!isNotMovable && targetableComponent.DistanceToTarget < fightingComponent.ReachZone)
                {
                    _inFightPool.Value.Add(entity);
                    viewComponent.Rigidbody.velocity = Vector3.zero;
                }
                else if(isNotMovable && targetableComponent.DistanceToTarget > fightingComponent.ReachZone)
                {
                    _inFightPool.Value.Del(entity);
                }
            }
        }
    }
}