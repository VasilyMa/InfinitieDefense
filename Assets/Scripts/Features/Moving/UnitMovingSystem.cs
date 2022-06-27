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
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var unitEntity in _allUnitsFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(unitEntity);
                ref var viewComponent = ref _viewPool.Value.Get(unitEntity);

                if (targetableComponent.TargetEntity == -1 || targetableComponent.TargetObject == null)
                {
                    viewComponent.Animator.SetBool("Run", false);
                    viewComponent.NavMeshAgent.ResetPath();
                    continue;
                }

                ref var movableComponent = ref _movablePool.Value.Get(unitEntity);
                ref var targetViewComponent = ref _viewPool.Value.Get(targetableComponent.TargetEntity);

                viewComponent.Animator.SetBool("Run", true);
                viewComponent.NavMeshAgent.SetDestination(targetableComponent.TargetObject.transform.position);
            }
        }
    }
}