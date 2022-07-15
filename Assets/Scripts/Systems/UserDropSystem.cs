using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class UserDropSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<Player, DropComponent>> _filterPlayer = default;
        public void Run (EcsSystems systems) {
            
        }
    }
}