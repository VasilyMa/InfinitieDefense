using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class HPRegenerationSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<HPRegeneration, HealthComponent>, Exc<DeadTag, InactiveTag>> _regenerationFilter = default;

        readonly EcsPoolInject<HPRegeneration> _regenerationPool = default;
        readonly EcsPoolInject<HealthComponent> _healthPool = default;
        public void Run (EcsSystems systems)
        {
            foreach (var regenerationEntity in _regenerationFilter.Value)
            {
                ref var regenerationComponent = ref _regenerationPool.Value.Get(regenerationEntity);
                ref var healthComponent = ref _healthPool.Value.Get(regenerationEntity);
                Debug.Log("����� � �����������");
                if (healthComponent.CurrentValue == healthComponent.MaxValue)
                {
                    Debug.Log("� ��� ��� ��");
                    continue;
                }

                if (healthComponent.CurrentValue < regenerationComponent.OldHPValue) // ���� ��� �������
                {
                    Debug.Log("�� ��� ������");
                    regenerationComponent.OldHPValue = healthComponent.CurrentValue;
                    regenerationComponent.CurrentCooldown = regenerationComponent.MaxCooldown;
                    continue;
                }

                regenerationComponent.CurrentCooldown -= Time.deltaTime;
                Debug.Log("��������� �����������");
                if (regenerationComponent.CurrentCooldown > 0)
                {
                    Debug.Log("����� ��� �� ������");
                    continue;
                }

                if (regenerationComponent.CurrentCooldown < 0)
                {
                    regenerationComponent.CurrentCooldown = 0;
                }

                healthComponent.CurrentValue += regenerationComponent.Value * Time.deltaTime;

                if (healthComponent.CurrentValue >= healthComponent.MaxValue)
                {
                    healthComponent.CurrentValue = healthComponent.MaxValue;
                }

                regenerationComponent.OldHPValue = healthComponent.CurrentValue;
                Debug.Log("�������������");
            }
        }
    }
}