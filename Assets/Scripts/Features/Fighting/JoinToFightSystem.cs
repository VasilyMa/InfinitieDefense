using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class JoinToFightSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable, ViewComponent, Movable, ReachZoneComponent>, Exc<InactiveTag>> _enemyFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ReachZoneComponent> _reachZonePool = default;
        readonly EcsPoolInject<InFightTag> _inFightPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var reachZoneComponent = ref _reachZonePool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                bool isInFigth = _inFightPool.Value.Has(entity);

                if (!isInFigth && targetableComponent.DistanceToTarget < reachZoneComponent.Value)
                {
                    _inFightPool.Value.Add(entity);
                    viewComponent.Rigidbody.velocity = Vector3.zero;
                    viewComponent.Animator.SetBool("Run", false);
                    viewComponent.Animator.SetBool("Attack", true);
                }
                else if(isInFigth && targetableComponent.DistanceToTarget > reachZoneComponent.Value)
                {
                    _inFightPool.Value.Del(entity);
                    viewComponent.Animator.SetBool("Attack", false);
                }
            }
        }
    }
}