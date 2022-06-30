using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StoneMiningSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<StoneMiningEvent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intComp = default;
        readonly EcsPoolInject<MoveToBagComponent> _moveToBagPool = default;
        readonly EcsFilterInject<Inc<OreMoveEvent>> _moveEventFilter = default;
        public void Run (EcsSystems systems) {

            foreach (var entity in _filter.Value)
            {
                foreach (var entityMove in _moveEventFilter.Value)
                {
                    _moveEventFilter.Pools.Inc1.Del(entityMove);
                }

                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                ref var intComp = ref _intComp.Value.Get(_state.Value.EntityInterface);
                ref var moveToBagComp = ref _moveToBagPool.Value.Add(entity);
                moveToBagComp.Transform = filterComp.StoneTransform;
                

                //_state.Value.StoneTransformList.Add(filterComp.StoneTransform);
                filterComp.StoneTransform.SetParent(playerComp.ResHolderTransform);
                //todo target
                moveToBagComp.StartPosition = moveToBagComp.Transform.localPosition;
                moveToBagComp.TargetPosition = new Vector3(0, _state.Value.RockCount * 0.6f, 0);
                moveToBagComp.Coin = false;

                // foreach(var item in _state.Value.CoinTransformList)
                // {
                //     item.localPosition = new Vector3(0, item.localPosition.y + 0.6f, 0);
                // }
                filterComp.StoneTransform.SetSiblingIndex(_state.Value.RockCount);
                _state.Value.RockCount++;
                // intComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateStone();
                //todo добавить перемещение камня за спину
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}