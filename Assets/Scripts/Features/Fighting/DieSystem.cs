using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DieSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnitTag, HealthComponent, ViewComponent>, Exc<DeadTag>> _unitsFilter = default;

        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _unitsFilter.Value)
            {
                if (_healthPool.Value.Get(entity).CurrentValue > 0)
                {
                    continue;
                }

                ref var viewComponent = ref _viewPool.Value.Get(entity);
                viewComponent.Animator.SetTrigger("Die");
                viewComponent.GameObject.layer = LayerMask.NameToLayer("Dead");
                _deadPool.Value.Add(entity);
            }
        }
    }
}