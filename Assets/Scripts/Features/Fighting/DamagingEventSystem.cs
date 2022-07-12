using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamagingEventSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<DamagingEvent>> _damagingEventFilter = default;

        readonly EcsPoolInject<DamagingEvent> _damagingEventPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        readonly EcsPoolInject<TargetingEvent> _targetingEventPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;

        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsPoolInject<DamagePopupEvent> _popupEvent = default;
        public void Run (EcsSystems systems)
        {
            foreach (var entity in _damagingEventFilter.Value)
            {
                ref var damagingEventComponent = ref _damagingEventPool.Value.Get(entity);

                if (damagingEventComponent.TargetEntity == -1)
                {
                    Debug.Log("��� ���������� DamagingEvent ������ -1 ������");
                    continue;
                }
                
                if (!_healthPool.Value.Has(damagingEventComponent.TargetEntity))
                {
                    continue;
                }

                ref var healthPointComponent = ref _healthPool.Value.Get(damagingEventComponent.TargetEntity);
                ref var viewComp = ref _viewPool.Value.Get(damagingEventComponent.TargetEntity);

                if (damagingEventComponent.DamageValue > healthPointComponent.CurrentValue)
                {
                    damagingEventComponent.DamageValue = healthPointComponent.CurrentValue;
                }

                healthPointComponent.CurrentValue -= damagingEventComponent.DamageValue;
                viewComp.Healthbar.UpdateHealth(healthPointComponent.CurrentValue);
                ref var popupComp = ref _popupEvent.Value.Add(_world.Value.NewEntity());
                popupComp.DamageAmount = (int)damagingEventComponent.DamageValue;
                popupComp.target = new Vector3(viewComp.Transform.position.x + Random.Range(-3, 3), viewComp.Transform.position.y + Random.Range(1, 4), viewComp.Transform.position.z + Random.Range(-3, 3));
                if (_targetablePool.Value.Has(damagingEventComponent.TargetEntity))
                {
                    ref var targetingEvent = ref _targetingEventPool.Value.Add(_world.Value.NewEntity());
                    targetingEvent.TargetEntity = damagingEventComponent.DamagingEntity;
                    targetingEvent.TargetingEntity = damagingEventComponent.TargetEntity;
                }
            }
        }
    }
}