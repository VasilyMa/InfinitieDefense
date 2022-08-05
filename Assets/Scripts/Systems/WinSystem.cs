using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client {
    sealed class WinSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<WinEvent>> _winFilter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsFilterInject<Inc<CountdownWaveComponent>> _countdownFilter = default;
        readonly EcsPoolInject<KillsCountComponent> _killsPool = default;
        readonly EcsSharedInject<GameState> _state;
        readonly EcsWorldInject _world = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _winFilter.Value)
            {
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                foreach (var item in _countdownFilter.Value)
                {
                    interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(0);
                    _countdownFilter.Pools.Inc1.Del(item); 
                }
                int index = 0;
                if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
                {
                    index = 2;
                }
                else
                {
                    index = SceneManager.GetActiveScene().buildIndex + 1;
                }
                _state.Value.Saves.SaveSceneNumber(index);
                _state.Value.Saves.SaveLevel(_state.Value.Saves.LVL + 1);
                //Time.timeScale = 0;
                interfaceComp.winPanel.SetActive(true);
                interfaceComp.killsCounter.SetActive(true);
                interfaceComp.winPanel.GetComponent<Animator>().SetTrigger("Win");
                interfaceComp.continueButton.SetActive(false);
                _killsPool.Value.Add(_world.Value.NewEntity());
                _winFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}