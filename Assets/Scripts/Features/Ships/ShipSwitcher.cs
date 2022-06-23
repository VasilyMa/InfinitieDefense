using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ShipSwitcher : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ShipTag, InactiveTag, CurrentWaveTag>> _InactiveShipsFilter = default;
        readonly EcsFilterInject<Inc<ShipTag, CurrentWaveTag>, Exc<InactiveTag>> _ActiveShipsFilter = default;
        readonly EcsFilterInject<Inc<EnemyTag, UnitTag>, Exc<DeadTag, InactiveTag>> _AliveUnitsFilter = default;
        readonly EcsFilterInject<Inc<EnemyTag, UnitTag, InactiveTag>, Exc<DeadTag>> _InactiveAliveUnitsFilter = default;

        readonly EcsPoolInject<ShipComponent> _shipPool = default;
        readonly EcsPoolInject<InactiveTag> _inactivePool = default;

        readonly EcsSharedInject<GameState> _state;

        public void Run (EcsSystems systems)
        {
            if (_ActiveShipsFilter.Value.GetEntitiesCount() > 0)
            {
                return;
            }

            if (_AliveUnitsFilter.Value.GetEntitiesCount() > 0)
            {
                return;
            }

            if (_InactiveAliveUnitsFilter.Value.GetEntitiesCount() == 0)
            {
                return;
            }

            Debug.Log(_state.Value.CurrentActivatedShip);

            foreach (var shipEntity in _InactiveShipsFilter.Value)
            {
                ref var shipComponent = ref _shipPool.Value.Get(shipEntity);
                if (shipComponent.Number == _state.Value.CurrentActivatedShip)
                {
                    _inactivePool.Value.Del(shipEntity);
                }
            }

            if (_InactiveShipsFilter.Value.GetEntitiesCount() > 0)
            {
                _state.Value.CurrentActivatedShip++;
            }

            Debug.Log(_state.Value.CurrentActivatedShip);
        }
    }
}