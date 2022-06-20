using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateDefenders : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateDefenderEvent>> _filter = default;
        readonly EcsPoolInject<MainTowerTag> _mainTowerPool = default;
        //todo components

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var mainTowerComp = ref _mainTowerPool.Value.Get(_state.Value.TowersEntity[0]);
                int count = _state.Value.TowerStorage.GetDefenderCountByID(_state.Value.DefenseTowers[0]);

                for (int i = 0; i < count;i++)
                {
                    if(_state.Value.DefendersGOs[i] == null)
                    {
                        _state.Value.DefendersGOs[i] = GameObject.Instantiate(_state.Value.TowerStorage.DefenderPrefab, mainTowerComp.DefendersPositions[i], Quaternion.identity);
                        //todo заполнить энтити дефендера
                    }
                }

                _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}