using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class LoseSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<LoseEvent>> _loseFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<DropByDie> _dropPool = default;
        readonly EcsSharedInject<GameState> _state;
        public void Run (EcsSystems systems) {
            foreach (var entity in _loseFilter.Value)
            {
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                _dropPool.Value.Add(_state.Value.EntityPlayer);
                //Time.timeScale = 0;
                //interfaceComp.losePanel.SetActive(true);
            }
        }
    }
}