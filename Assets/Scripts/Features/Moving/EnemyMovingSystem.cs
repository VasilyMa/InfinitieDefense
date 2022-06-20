using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemyMovingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EnemyTag, ViewComponent, Movable, Targetable>, Exc<InFightTag, InactiveTag, DeadTag>> _allEnemyFilter = default;

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

                if (viewComponent.Rigidbody.isKinematic)
                {
                    Vector3 flatTargetPosition = new Vector3(targetableComponent.TargetObject.transform.position.x,
                                                                viewComponent.GameObject.transform.position.y,
                                                                targetableComponent.TargetObject.transform.position.z);

                    viewComponent.GameObject.transform.position = Vector3.MoveTowards(  viewComponent.GameObject.transform.position,
                                                                                        flatTargetPosition,
                                                                                        movableComponent.Speed * Time.deltaTime);
                }
                else
                {
                    Vector3 direction = (targetableComponent.TargetObject.transform.position - viewComponent.GameObject.transform.position).normalized * movableComponent.Speed;

                    viewComponent.Animator.SetBool("Run", true);
                    viewComponent.Rigidbody.velocity = new Vector3(direction.x, viewComponent.Rigidbody.velocity.y, direction.z);
                }
            }
        }
    }
}