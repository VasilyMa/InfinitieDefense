using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class TestGuiInitSystem : IEcsInitSystem {
        EcsPool<EcsUguiClickEvent> _clickEventsPool;
        EcsFilter _clickEvents;
        public void Init (EcsSystems systems) {
            var world = systems.GetWorld();
            _clickEventsPool = world.GetPool<EcsUguiClickEvent>();
            _clickEvents = world.Filter<EcsUguiClickEvent>().End();
        }
    }
}