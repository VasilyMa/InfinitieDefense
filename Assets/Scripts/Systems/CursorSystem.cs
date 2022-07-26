using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class CursorSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<CursorEvent>> _cursorFilter = default;
        readonly EcsPoolInject<OreComponent> _orePool = default;
        public void Run (EcsSystems systems) {

        }
    }
}