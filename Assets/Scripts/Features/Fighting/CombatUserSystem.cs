using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CombatUserSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsFilterInject<Inc<CombatEventComponent, HealthComponent, ViewComponent>> _filter = default;
        readonly EcsPoolInject<Player> _player = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var healthComponent = ref _filter.Pools.Inc2.Get(entity);
                ref var player = ref _player.Value.Get(_state.Value.EntityPlayer);
                healthComponent.CurrentValue -= player.damage;
                Debug.Log(healthComponent.CurrentValue);
                if (healthComponent.CurrentValue <= 0)
                {
                    _filter.Pools.Inc2.Del(entity);
                    _filter.Pools.Inc3.Del(entity);
                }
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}