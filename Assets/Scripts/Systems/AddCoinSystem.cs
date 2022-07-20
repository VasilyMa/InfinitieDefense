using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class AddCoinSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;

        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<AddCoinEvent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intPool = default;
        readonly EcsPoolInject<MoveToBagComponent> _moveToBagPool = default;

        readonly EcsPoolInject<VibrationEvent> _vibrationEventPool = default;

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                ref var intComp = ref _intPool.Value.Get(_state.Value.EntityInterface);
                ref var moveToBagComp = ref _moveToBagPool.Value.Add(entity);
                moveToBagComp.Transform = filterComp.CoinTransform;

                filterComp.CoinTransform.SetParent(playerComp.ResHolderTransform);
                moveToBagComp.StartPosition = moveToBagComp.Transform.localPosition;
                moveToBagComp.TargetPosition = new Vector3(0, _state.Value.CoinCount * 0.3f + _state.Value.RockCount * 0.6f, 0);
                //_state.Value.CoinCount++;
                moveToBagComp.Coin = true;

                // _state.Value.CoinTransformList.Add(filterComp.CoinTransform);
                filterComp.CoinTransform.SetSiblingIndex(_state.Value.CoinCount + _state.Value.RockCount);
                _state.Value.CoinCount++;
                _vibrationEventPool.Value.Add(_world.Value.NewEntity()).Vibration = VibrationEvent.VibrationType.LightImpact;
                // intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}