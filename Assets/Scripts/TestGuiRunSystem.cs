using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
namespace Client {
    sealed class TestGuiRunSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<EcsUguiClickEvent>> _clickEvent = default;
        readonly EcsPoolInject<EcsUguiClickEvent> _clickPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _clickEvent.Value)
            {
                ref EcsUguiClickEvent data = ref _clickPool.Value.Get(entity);
                Debug.Log("Click!");
            }
        }
    }
}