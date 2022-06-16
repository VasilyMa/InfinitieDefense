using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class RadiusInitSystem : IEcsInitSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<RadiusComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        
        public void Init (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                
                ref var radiusComp = ref _filter.Pools.Inc1.Get(entity);
                ref var viewComp = ref _viewPool.Value.Get(entity);
                if (viewComp.GameObject != null)
                {
                    radiusComp.RadiusTransform = GameObject.Instantiate(_state.Value.InterfaceStorage.RadiusPrefab, viewComp.GameObject.transform).GetComponent<Transform>();
                    radiusComp.RadiusTransform.localScale = new Vector3(radiusComp.Radius * 2, radiusComp.Radius * 2, 1);
                }
            }
        }
    }
}