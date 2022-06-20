using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DieSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<UnitTag, HealthComponent, ViewComponent>, Exc<DeadTag, InactiveTag>> _unitsFilter = default;

        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;
        readonly EcsPoolInject<DroppedGoldEvent> _goldPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _unitsFilter.Value)
            {
                if (_healthPool.Value.Get(entity).CurrentValue > 0)
                {
                    continue;
                }


                ref var viewComponent = ref _viewPool.Value.Get(entity);
                Debug.Log("Обработка " + viewComponent.GameObject.name);
                if (viewComponent.Rigidbody) viewComponent.Rigidbody.velocity = Vector3.zero;
                if (viewComponent.Animator) viewComponent.Animator.SetTrigger("Die");
                viewComponent.GameObject.layer = LayerMask.NameToLayer("Dead");

                ref var goldComp = ref _goldPool.Value.Add(_world.Value.NewEntity());
                if (viewComponent.Transform) goldComp.Position = viewComponent.Transform.position;

                if (viewComponent.Outline) viewComponent.Outline.enabled = false;

                _deadPool.Value.Add(entity);
            }
        }
    }
}