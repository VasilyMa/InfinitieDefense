using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EncounterSwitcher : IEcsRunSystem
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
                Debug.Log("Всё ещё существуют активные корабли");
                return;
            }

            if (_AliveUnitsFilter.Value.GetEntitiesCount() > 0)
            {
                Debug.Log("Всё ещё есть живые юниты");
                return;
            }

            if (_InactiveAliveUnitsFilter.Value.GetEntitiesCount() == 0)
            {
                Debug.Log("Закончились неактивные юниты");
                return;
            }

            Debug.Log("Начали активацию корабля на энкаунтере "+_state.Value.CurrentActivatedShip);

            foreach (var shipEntity in _InactiveShipsFilter.Value)
            {
                ref var shipComponent = ref _shipPool.Value.Get(shipEntity);
                if (shipComponent.Encounter == _state.Value.CurrentActivatedShip)
                {
                    _inactivePool.Value.Del(shipEntity);
                    Debug.Log("Активировали корабль на энкаунтере " + _state.Value.CurrentActivatedShip);
                }
            }

            if (_InactiveShipsFilter.Value.GetEntitiesCount() > 0)
            {
                Debug.Log("Есть неактивные корабли, поэтому сменяем энкаунтер");
                _state.Value.CurrentActivatedShip++;
                Debug.Log("Сменили энкаунтер на " + _state.Value.CurrentActivatedShip);
            }

            Debug.Log("Закончили работу. Теперь активный энкаунтер - №"+_state.Value.CurrentActivatedShip);
        }
    }
}