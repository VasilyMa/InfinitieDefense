using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DieSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<UnitTag, HealthComponent, ViewComponent>, Exc<DeadTag>> _unitsFilter = default;

        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;
        readonly EcsPoolInject<DroppedGoldEvent> _goldPool = default;
        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _unitsFilter.Value)
            {
                if (_healthPool.Value.Get(entity).CurrentValue > 0)
                {
                    continue;
                }

                ref var viewComponent = ref _viewPool.Value.Get(entity);
                viewComponent.Rigidbody.velocity = Vector3.zero;
                viewComponent.Animator.SetTrigger("Die");
                viewComponent.GameObject.layer = LayerMask.NameToLayer("Dead");

                ref var goldComp = ref _goldPool.Value.Add(_world.Value.NewEntity());
                goldComp.Position = viewComponent.Transform.position;

                if (viewComponent.Outline) viewComponent.Outline.enabled = false;

                if (_movablePool.Value.Has(entity)) _movablePool.Value.Del(entity);
                if (_targetablePool.Value.Has(entity)) _targetablePool.Value.Del(entity);

                _deadPool.Value.Add(entity);
            }
        }
    }
}