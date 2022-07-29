using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DamagingEventSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsSharedInject<GameState> _state;

        readonly EcsFilterInject<Inc<DamagingEvent>> _damagingEventFilter = default;

        readonly EcsPoolInject<DamagingEvent> _damagingEventPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DestroyEffects> _destroyEffectsPool = default;

        readonly EcsPoolInject<TargetingEvent> _targetingEventPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<DamagePopupEvent> _popupEvent = default;
        readonly EcsPoolInject<VibrationEvent> _vibrationPool = default;

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

                GameObject popup = null;
                bool popupIsOver = true;
                foreach (var item in viewComp.DamagePopups)
                {
                    if (!item.activeSelf)
                    {
                        popup = item;
                        popupIsOver = false;
                        break;
                    }
                }
                if (!popupIsOver)
                {
                    ref var popupComp = ref _popupEvent.Value.Add(_world.Value.NewEntity());
                    //popup.gameObject.transform.position = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 2f, viewComp.GameObject.transform.position.z);
                    //popup.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                    popupComp.DamageAmount = (int)damagingEventComponent.DamageValue;
                    popupComp.target = new Vector3(viewComp.Transform.position.x + Random.Range(-3, 3), viewComp.Transform.position.y + Random.Range(3, 6), viewComp.Transform.position.z + Random.Range(-3, 3));
                    popupComp.DamageObject = popup;
                    popupComp.DamageObject.transform.position = new Vector3(viewComp.GameObject.transform.position.x, viewComp.GameObject.transform.position.y + 2f, viewComp.GameObject.transform.position.z);
                    popupComp.DamageObject.transform.localScale = new Vector3(0.01f, 0.01f, 1);
                    popupComp.timeOut = 1.5f;
                }

                if (damagingEventComponent.DamagingEntity == _state.Value.EntityPlayer)
                {
                    ref var vibrationEvent = ref _vibrationPool.Value.Add(entity);
                    vibrationEvent.Vibration = VibrationEvent.VibrationType.MediumImpact;
                }

                if (_targetablePool.Value.Has(damagingEventComponent.TargetEntity))
                {
                    ref var targetingEvent = ref _targetingEventPool.Value.Add(_world.Value.NewEntity());
                    targetingEvent.TargetEntity = damagingEventComponent.DamagingEntity;
                    targetingEvent.TargetingEntity = damagingEventComponent.TargetEntity;
                }

                IncreaseFireEffect(damagingEventComponent.TargetEntity, healthPointComponent.CurrentValue, healthPointComponent.MaxValue);
            }
        }

        private void IncreaseFireEffect(in int entity, in float currentHP, in float maxHP)
        {
            if (_destroyEffectsPool.Value.Has(entity))
            {
                ref var destroyEffectsComponent = ref _destroyEffectsPool.Value.Get(entity);

                float maxFireValue = 3;
                float fireMultiply = 1 - (currentHP / maxHP);

                // 4 stage of fire: 25%, 50%, 75%, 100%

                if (fireMultiply < 0.25f) fireMultiply = 0f;
                else if (fireMultiply > 0.25f && fireMultiply < 0.5f) fireMultiply = 0.25f;
                else if (fireMultiply > 0.5f && fireMultiply < 0.75f) fireMultiply = 0.5f;
                else if (fireMultiply > 0.75f && fireMultiply < 1f) fireMultiply = 0.75f;
                else fireMultiply = 1f;

                if (fireMultiply != 0)
                {
                    if (destroyEffectsComponent.DestroyFire.isStopped) destroyEffectsComponent.DestroyFire.Play();
                }

                destroyEffectsComponent.DestroyFire.startSize = maxFireValue * fireMultiply;
            }
        }
    }
}