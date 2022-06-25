using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TowerShotSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<TowerTag, InFightTag, Targetable, Cooldown>, Exc<DeadTag, InactiveTag, UnitTag>> _towerFilter = default;

        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Cooldown> _cooldownPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<DamageComponent> _damagePool = default;
        readonly EcsPoolInject<Projectile> _projectilePool = default;

        readonly EcsSharedInject<GameState> _state = default;

        public void Run (EcsSystems systems)
        {
            foreach (var towerEntity in _towerFilter.Value)
            {
                ref var cooldownComponent = ref _cooldownPool.Value.Get(towerEntity);
                ref var viewComponent = ref _viewPool.Value.Get(towerEntity);
                ref var targetableComponent = ref _targetablePool.Value.Get(towerEntity);

                // Вспомогательные поля
                float startDistance = Vector3.Distance(viewComponent.TowerFirePoint.transform.position, targetableComponent.TargetObject.transform.position);
                Vector3 positionBetween = Vector3.Lerp(targetableComponent.TargetObject.transform.position, viewComponent.TowerFirePoint.transform.position, 0.5f);
                Vector3 supportPosition =  new Vector3(positionBetween.x, viewComponent.TowerFirePoint.transform.position.y + startDistance * 0.5f, positionBetween.z);

                Vector3 towerFirePointOffset = new Vector3(90, 0, 0);

                viewComponent.TowerFirePoint.transform.LookAt(supportPosition);
                viewComponent.TowerFirePoint.transform.Rotate(towerFirePointOffset);

                if (cooldownComponent.CurrentValue > 0)
                {
                    continue;
                }

                int cannonBallEntity = _world.Value.NewEntity();

                ref var cannonBallViewComponent = ref _viewPool.Value.Add(cannonBallEntity);
                ref var projectileComponent = ref _projectilePool.Value.Add(cannonBallEntity);
                ref var damageComponent = ref _damagePool.Value.Add(cannonBallEntity);


                cannonBallViewComponent.GameObject = GameObject.Instantiate(Resources.Load<GameObject>("Test"), viewComponent.TowerFirePoint.transform.position, Quaternion.identity);

                projectileComponent.Speed = 30;
                projectileComponent.SpeedDecreaseFactor = 1.2f;
                projectileComponent.SpeedIncreaseFactor = 0.8f;
                projectileComponent.OwnerEntity = towerEntity;
                projectileComponent.StartPosition = viewComponent.TowerFirePoint.transform.position;
                projectileComponent.SupportPosition = supportPosition;
                projectileComponent.TargetEntity = targetableComponent.TargetEntity;
                projectileComponent.TargetObject = targetableComponent.TargetObject;

                GameObject.Instantiate(Resources.Load<GameObject>("Test"), supportPosition, Quaternion.identity);

                damageComponent.Value = 1000;

                cooldownComponent.CurrentValue = cooldownComponent.MaxValue;
            }
        }
    }
}