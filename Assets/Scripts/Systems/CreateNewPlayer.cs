using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNewPlayer : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateNewPlayerEvent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        //readonly EcsPoolInject<Fil

        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                
            }
        }
    }
}