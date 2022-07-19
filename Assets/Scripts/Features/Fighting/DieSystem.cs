using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DieSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<HealthComponent, ViewComponent>, Exc<DeadTag, InactiveTag>> _unitsFilter = default;

        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<MainTowerTag> _mainTowerPool = default;
        readonly EcsPoolInject<Resurrectable> _resurrectablePool = default;
        readonly EcsPoolInject<RespawnEvent> _respawnEventPool = default;
        readonly EcsPoolInject<DefenderComponent> _defenderPool = default;
        readonly EcsPoolInject<DropByDie> _dropPool = default;

        readonly EcsPoolInject<LoseEvent> _losePool = default;
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
                ref var interfaceComponent = ref _interfacePool.Value.Get(_state.Value.EntityInterface); ;
                if (viewComponent.GameObject) viewComponent.GameObject.layer = LayerMask.NameToLayer("Dead");
                if (viewComponent.Rigidbody) viewComponent.Rigidbody.velocity = Vector3.zero;
                if (viewComponent.Animator) viewComponent.Animator.SetTrigger("Die");

                if (_enemyPool.Value.Has(entity))
                {
                    ref var goldComp = ref _goldPool.Value.Add(_world.Value.NewEntity());
                    if (viewComponent.Transform) goldComp.Position = viewComponent.Transform.position;
                }

                if (viewComponent.Outline) viewComponent.Outline.enabled = false;
                if (viewComponent.NavMeshAgent) viewComponent.NavMeshAgent.enabled = false;
                if (viewComponent.Healthbar) viewComponent.Healthbar.Disable();

                _deadPool.Value.Add(entity);

                if (_enemyPool.Value.Has(entity)) interfaceComponent.progressbar.GetComponent<ProgressBarMB>().UpdateProgressBar();

                if (_mainTowerPool.Value.Has(entity)) _losePool.Value.Add(_world.Value.NewEntity());

                if (_resurrectablePool.Value.Has(entity))
                {
                    _respawnEventPool.Value.Add(entity);
                }
                if (_deadPool.Value.Has(_state.Value.EntityPlayer))
                {
                    _dropPool.Value.Add(_state.Value.EntityPlayer);
                }
            }
        }
    }
}