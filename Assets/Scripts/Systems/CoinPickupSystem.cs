using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CoinPickupSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CoinPickupEvent>> _filterCoinPickup = default;
        readonly EcsPoolInject<CoinPickupEvent> _coinPool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filterCoinPickup.Value)
            {
                ref var camerComp = ref _cameraPool.Value.Get(_state.Value.EntityCamera);
                ref var coinComp = ref _coinPool.Value.Get(entity);
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                float speed = coinComp.Speed;
                coinComp.CoinObject.transform.position = Vector3.MoveTowards(coinComp.CoinObject.transform.position, new Vector3(camerComp.HolderTransform.position.x + 12, camerComp.HolderTransform.position.y + 7, camerComp.HolderTransform.position.z), speed * Time.deltaTime);
                
                if (coinComp.CoinObject.transform.position.y >= camerComp.HolderTransform.position.y + 7)
                {
                    _state.Value.CoinCount++;
                    interfaceComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                    GameObject.Destroy(coinComp.CoinObject);
                    _filterCoinPickup.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}