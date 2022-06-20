using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddCoinSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<AddCoinEvent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                ref var intComp = ref _intPool.Value.Get(_state.Value.EntityInterface);
                filterComp.CoinTransform.SetParent(playerComp.ResHolderTransform);
                filterComp.CoinTransform.localPosition = new Vector3(0, _state.Value.CoinCount + _state.Value.RockCount, 0);

                _state.Value.CoinTransformList.Add(filterComp.CoinTransform);
                filterComp.CoinTransform.SetSiblingIndex(_state.Value.CoinCount + _state.Value.RockCount);
                _state.Value.CoinCount++;
                intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}