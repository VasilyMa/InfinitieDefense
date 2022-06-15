using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class StoneMiningSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<StoneMiningEvent>> _filter = default;
        readonly EcsPoolInject<Player> _playerPool = default;


        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var playerComp = ref _playerPool.Value.Get(_state.Value.EntityPlayer);
                _state.Value.StoneTransformList.Add(filterComp.StoneTransform);
                filterComp.StoneTransform.SetParent(playerComp.ResHolderTransform);
                filterComp.StoneTransform.localPosition = new Vector3(0, _state.Value.RockCount, 0);

                foreach(var item in _state.Value.CoinTransformList)
                {
                    item.localPosition = new Vector3(0, item.localPosition.y + 1, 0);
                }
                filterComp.StoneTransform.SetSiblingIndex(_state.Value.RockCount);
                _state.Value.RockCount++;
                //todo добавить перемещение камня за спину
                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}