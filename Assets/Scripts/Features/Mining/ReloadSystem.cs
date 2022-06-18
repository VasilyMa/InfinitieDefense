using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class ReloadSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<CooldownComponent, ReloadComponent>> _filter = default;
        public void Run(EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var cooldownComp = ref _filter.Pools.Inc1.Get(entity);
                ref var curCooldown = ref cooldownComp.currentValue;
                ref var maxCooldown = ref cooldownComp.maxValue;

                if (curCooldown > 0)
                    curCooldown -= Time.deltaTime;
                else if (curCooldown <= 0)
                {
                    curCooldown = 0;
                    _filter.Pools.Inc2.Del(entity);
                }
            }
        }
    }
}