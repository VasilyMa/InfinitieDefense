using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine; 
using UnityEngine.UI;
namespace Client {
    sealed class UpgradeCanvasSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state;
        readonly EcsFilterInject<Inc<CanvasUpgradeComponent>> _filter = default;
        public void Run (EcsSystems systems) {

            foreach (var entity in _filter.Value)
            {
                ref var canvasComp = ref _filter.Pools.Inc1.Get(entity);
                canvasComp.upgrade.UpdateUpgradePoint(_state.Value.TowersUpgrade[canvasComp.Index], _state.Value.DefenseTowerStorage.GetUpgradeByID(_state.Value.DefenseTowers[canvasComp.Index]));
            }
            
        }
    }
}