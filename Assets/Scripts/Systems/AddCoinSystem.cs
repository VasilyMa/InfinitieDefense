using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddCoinSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<AddCoinEvent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);

                filterComp.CoinTransform.SetParent(playerComp.ResHolderTransform);
                filterComp.CoinTransform.localPosition = new Vector3(0, _state.Value.CoinCount, 0);

                _state.Value.CoinCount++;
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}