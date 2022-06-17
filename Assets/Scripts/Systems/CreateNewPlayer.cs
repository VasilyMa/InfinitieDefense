using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;

namespace Client {
    sealed class CreateNewPlayer : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CreateNewPlayerEvent>> _filter = default;
        
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {

            }
        }
    }
}