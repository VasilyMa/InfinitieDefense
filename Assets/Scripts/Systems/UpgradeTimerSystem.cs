using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class UpgradeTimerSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UpgradeTimerEvent>> _filterTimer = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _canvasPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filterTimer.Value)
            {
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                ref var timerComp = ref _filterTimer.Pools.Inc1.Get(entity);
                if (interfaceComp._joystick.Horizontal == 0 || interfaceComp._joystick.Vertical == 0)
                {
                    if (timerComp.TimeToUpgrade < 4f)
                    {
                        _canvasPool.Value.Get(timerComp.Entity).timerResources.ResourcesDrop(timerComp.TimeToUpgrade/2);
                        timerComp.TimeToUpgrade += Time.deltaTime * 2f;
                    }
                    else if (timerComp.TimeToUpgrade >= 4f)
                    {
                        _canvasPool.Value.Get(timerComp.Entity).timerResources.ResourcesDrop(0);
                        _filterTimer.Pools.Inc1.Del(entity);
                    }
                }
            }
        }
    }
}