using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamagingEventSystem : IEcsRunSystem
    {

        readonly EcsFilterInject<Inc<DamagingEventComponent>> _damagingEventFilter = default;

        readonly EcsPoolInject<DamagingEventComponent> _damagingEventPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;

        readonly EcsSharedInject<GameState> _state = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _damagingEventFilter.Value)
            {
                ref var damagingEventComponent = ref _damagingEventPool.Value.Get(entity);

                if (!_healthPool.Value.Has(damagingEventComponent.TargetingEntity))
                {
                    _damagingEventPool.Value.Del(entity);
                    continue;
                }

                ref var healthPointComponent = ref _healthPool.Value.Get(damagingEventComponent.TargetingEntity);

                if (damagingEventComponent.DamageValue > healthPointComponent.CurrentValue)
                {
                    damagingEventComponent.DamageValue = healthPointComponent.CurrentValue;
                }

                healthPointComponent.CurrentValue -= damagingEventComponent.DamageValue;
            }
        }
    }
}