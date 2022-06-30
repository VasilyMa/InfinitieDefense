using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckCircleMiningSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world;
        readonly EcsFilterInject<Inc<CircleComponent>> _circleFilter = default;
        readonly EcsFilterInject<Inc<MainTowerTag, ViewComponent>> _mainTowerFilter = default;
        readonly EcsPoolInject<CircleComponent> _ciclePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _circleFilter.Value)
            {
                ref var playerComp = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                ref var circleComp = ref _ciclePool.Value.Get(entity);
                
                foreach (var entityMainTower in _mainTowerFilter.Value)
                {
                    ref var mainTowerComp = ref _viewPool.Value.Get(entityMainTower);
                    var distance = Vector3.Distance(playerComp.GameObject.transform.position, mainTowerComp.GameObject.transform.position);
                    if (distance >= circleComp.maxDistance)
                    {
                        playerComp.CanMining = false;

                        Debug.Log("Хрен а не майнинг");
                    }
                    else
                    {
                        playerComp.CanMining = true;

                        Debug.Log("Можно по майнить");
                    }
                }

            }
        }
    }
}