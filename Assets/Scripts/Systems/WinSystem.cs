using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class WinSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<WinEvent>> _winFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsFilterInject<Inc<CountdownWaveComponent>> _countdownFilter = default;
        readonly EcsSharedInject<GameState> _state; 
        public void Run (EcsSystems systems) {
            foreach (var entity in _winFilter.Value)
            {
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                foreach (var item in _countdownFilter.Value)
                {
                    interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(0);
                    _countdownFilter.Pools.Inc1.Del(item); 
                }
                
                Time.timeScale = 0;
                interfaceComp.winPanel.SetActive(true);
                interfaceComp.winPanel.GetComponent<Animator>().SetTrigger("Win");
                interfaceComp.continueButton.SetActive(false);
                _winFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}