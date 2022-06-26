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

                if (_enemyPool.Value.Has(unitEntity))
                {
                    viewComponent.Animator.SetBool("Run", true);
                    viewComponent.NavMeshAgent.SetDestination(targetableComponent.TargetObject.transform.position);
                    Debug.Log(targetableComponent.TargetObject);
                    Debug.Log(targetableComponent.TargetObject.transform.position);
                    Debug.Log("Установили точку назначения");
                }
                else
                {
                    Vector3 direction = (targetViewComponent.GameObject.transform.position - viewComponent.GameObject.transform.position).normalized * movableComponent.Speed;

                    viewComponent.Animator.SetBool("Run", true);
                    viewComponent.Rigidbody.velocity = new Vector3(direction.x, viewComponent.Rigidbody.velocity.y, direction.z);
                }
            }
        }
    }
}