using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitEnemyUnits : IEcsInitSystem
    {
        readonly EcsWorldInject _world = default;

        EcsSharedInject<GameState> _state = default;

        private string Enemy;

        public void Init(EcsSystems systems)
        {
            var allEnemyUnits = GameObject.FindGameObjectsWithTag(nameof(Enemy));

            var world = systems.GetWorld();

            foreach (var enemy in allEnemyUnits)
            {
                var enemyEntity = world.NewEntity();

                world.GetPool<EnemyTag>().Add(enemyEntity);

                world.GetPool<UnitTag>().Add(enemyEntity);

                world.GetPool<InactiveTag>().Add(enemyEntity);

                ref var targetableComponent = ref world.GetPool<Targetable>().Add(enemyEntity);
                ref var movableComponent = ref world.GetPool<Movable>().Add(enemyEntity);
                ref var viewComponent = ref world.GetPool<ViewComponent>().Add(enemyEntity);
                ref var shipComponent = ref world.GetPool<ShipComponent>().Add(enemyEntity);
                ref var healthComponent = ref world.GetPool<HealthComponent>().Add(enemyEntity);
                ref var reachZoneComponent = ref world.GetPool<ReachZoneComponent>().Add(enemyEntity);
                ref var damageComponent = ref world.GetPool<DamageComponent>().Add(enemyEntity);

                healthComponent.MaxValue = 100;
                healthComponent.CurrentValue = healthComponent.MaxValue;

                reachZoneComponent.Value = 5f;

                damageComponent.Value = 5f;

                movableComponent.Speed = 10f;

                viewComponent.GameObject = enemy;
                viewComponent.Rigidbody = enemy.GetComponent<Rigidbody>();
                viewComponent.Animator = enemy.GetComponent<Animator>();
                viewComponent.Transform = enemy.GetComponent<Transform>();
                viewComponent.Outline = enemy.GetComponent<Outline>();
                viewComponent.AttackMonoBehaviour = enemy.GetComponent<AttackMonoBehaviour>();
                viewComponent.AttackMonoBehaviour.Init(_world);
                viewComponent.AttackMonoBehaviour.SetEntity(enemyEntity);
                viewComponent.AttackMonoBehaviour.SetDamageValue(damageComponent.Value);
                viewComponent.EcsInfoMB = enemy.GetComponent<EcsInfoMB>();
                viewComponent.EcsInfoMB.Init(_world);
                viewComponent.EcsInfoMB.SetEntity(enemyEntity);

                shipComponent.Number = viewComponent.GameObject.transform.parent.GetComponent<ShipArrivalMonoBehavior>().GetShipNumber();
            }
        }
    }
}