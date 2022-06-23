using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class UnitMovingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnitTag, ViewComponent, Movable, Targetable>, Exc<InFightTag, InactiveTag, DeadTag, IsNotMovableTag>> _allUnitsFilter = default;

        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var unitEntity in _allUnitsFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(unitEntity);

                if (targetableComponent.TargetEntity == -1)
                {
                    continue;
                }

                ref var movableComponent = ref _movablePool.Value.Get(unitEntity);

                ref var viewComponent = ref _viewPool.Value.Get(unitEntity);
                ref var targetViewComponent = ref _viewPool.Value.Get(targetableComponent.TargetEntity);

                Vector3 direction = (targetViewComponent.GameObject.transform.position - viewComponent.GameObject.transform.position).normalized * movableComponent.Speed;

                viewComponent.Animator.SetBool("Run", true);
                viewComponent.Rigidbody.velocity = new Vector3(direction.x, viewComponent.Rigidbody.velocity.y, direction.z);
            }
        }
    }
}