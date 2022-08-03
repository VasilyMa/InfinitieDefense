using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class TowerRecoveryEventSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TowerRecoveryEvent>> _towerRecoveryEventFilter = default;

        readonly EcsPoolInject<TowerRecoveryEvent> _towerRecoveryPool = default;
        readonly EcsPoolInject<HealthComponent> _healthComponentPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;

        private float _healingPercentPerEvent = 0.05f;

        public void Run (EcsSystems systems)
        {
            foreach (var eventEntity in _towerRecoveryEventFilter.Value)
            {
                ref var towerRecoveryEvent = ref _towerRecoveryPool.Value.Get(eventEntity);

                if (_deadPool.Value.Has(eventEntity))
                {
                    _towerRecoveryPool.Value.Del(eventEntity);
                    continue;
                }

                ref var healthComponent = ref _healthComponentPool.Value.Get(towerRecoveryEvent.TowerEntity);
                ref var viewComponent = ref _viewPool.Value.Get(towerRecoveryEvent.TowerEntity);

                healthComponent.CurrentValue += healthComponent.MaxValue * _healingPercentPerEvent;

                if (healthComponent.CurrentValue > healthComponent.MaxValue)
                {
                    healthComponent.CurrentValue = healthComponent.MaxValue;
                }

                viewComponent.Healthbar?.SetHealth(healthComponent.CurrentValue);
                _towerRecoveryPool.Value.Del(eventEntity);
            }
        }
    }
}