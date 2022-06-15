using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNextTowerSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateNextTowerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<RadiusComponent> _radiusPool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                
                ref var radiusComp = ref _radiusPool.Value.Get(entity);

                ref var viewComp = ref _viewPool.Value.Get(entity);

                GameObject.Destroy(viewComp.GameObject);
                _state.Value.CurrentTowerID = _state.Value.TowerStorage.GetNextIDByID(_state.Value.CurrentTowerID);
                viewComp.GameObject = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID(_state.Value.CurrentTowerID),Vector3.zero, Quaternion.identity);

                radiusComp.Radius = _state.Value.TowerStorage.GetRadiusByID(_state.Value.CurrentTowerID);
                radiusComp.RadiusTransform.localScale = new Vector3(radiusComp.Radius, radiusComp.Radius, 1);

                _filter.Pools.Inc1.Del(entity);

            }
        }
    }
}