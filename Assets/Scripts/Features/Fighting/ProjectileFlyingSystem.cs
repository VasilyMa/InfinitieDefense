using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ProjectileFlyingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Projectile, DamageComponent, ViewComponent>> _projectileFilter = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<Projectile> _projectilePool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var projectileEntity in _projectileFilter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(projectileEntity);
                ref var projectileComponent = ref _projectilePool.Value.Get(projectileEntity);
                ref var damageComponent = ref _damagePool.Value.Get(projectileEntity);

                float distanceOverall = Vector3.Distance(projectileComponent.StartPosition, projectileComponent.TargetObject.transform.position);

                float distanceCovered = Vector3.Distance(projectileComponent.StartPosition, viewComponent.GameObject.transform.position);

                float distanceLeft = Vector3.Distance(viewComponent.GameObject.transform.position, projectileComponent.TargetObject.transform.position);

                if ((distanceOverall > distanceCovered + distanceLeft) || (distanceOverall < Mathf.Abs(distanceCovered - distanceLeft)))
                {
                    // Здесь могла быть ваша ошибка
                    Debug.LogError("Произошёл прикек с рассчетом координат для летящего снаряда");
                    continue;
                }

                Vector3 relativePos = (projectileComponent.SupportPosition - viewComponent.GameObject.transform.position);
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                viewComponent.GameObject.transform.rotation = rotation;

                float speed;

                if (viewComponent.GameObject.transform.position.y > projectileComponent.SupportPosition.y)
                {
                    speed = projectileComponent.Speed * projectileComponent.SpeedDecreaseFactor * Time.deltaTime;
                }
                else
                {
                    speed = projectileComponent.Speed * projectileComponent.SpeedIncreaseFactor * Time.deltaTime;
                }

                projectileComponent.SupportPosition = Vector3.MoveTowards(projectileComponent.SupportPosition,
                projectileComponent.TargetObject.transform.position, speed);
                viewComponent.GameObject.transform.position = Vector3.MoveTowards(viewComponent.GameObject.transform.position,
                projectileComponent.SupportPosition, speed);

                if (viewComponent.GameObject.transform.position == projectileComponent.TargetObject.transform.position)
                {
                    viewComponent.GameObject.SetActive(false);
                }
            }
        }
    }
}