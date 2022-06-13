using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitMainTower : IEcsInitSystem
    {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world = default;
        readonly EcsPoolInject<MainTowerTag> _towerPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Init (EcsSystems systems)
        {
            var mainTowerEntity = _world.Value.NewEntity();
            _state.Value.EntityMainTower = mainTowerEntity;

            _towerPool.Value.Add(mainTowerEntity);

            var mainTower = GameObject.Instantiate(_state.Value.TowerStorage.GetTowerPrefabByID("1tower"), new Vector3(0,10,0), Quaternion.identity);

            ref var viewComponent = ref _viewPool.Value.Add(mainTowerEntity);
            viewComponent.GameObject = mainTower;
            
        }
    }
}