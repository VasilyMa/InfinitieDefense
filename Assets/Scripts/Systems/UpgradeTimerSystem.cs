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
                    if (timerComp.TimeToUpgrade < _state.Value.DelayBeforUpgrade)
                    {
                        float timerFilling = timerComp.TimeToUpgrade / _state.Value.DelayBeforUpgrade;

                        _canvasPool.Value.Get(timerComp.Entity).timerResources.ResourcesDrop(timerFilling);
                        timerComp.TimeToUpgrade += Time.deltaTime;
                    }
                    else if (timerComp.TimeToUpgrade >= _state.Value.DelayBeforUpgrade)
                    {
                        _canvasPool.Value.Get(timerComp.Entity).timerResources.ResourcesDrop(0);
                    }
                }
                else
                {
                    timerComp.TimeToUpgrade = 0f;
                    _canvasPool.Value.Get(timerComp.Entity).timerResources.ResourcesDrop(0);
                }
            }
        }
    }
}