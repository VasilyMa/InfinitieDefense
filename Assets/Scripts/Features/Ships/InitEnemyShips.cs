using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class InitEnemyShips : IEcsInitSystem
    {
        readonly EcsWorldInject _world = default;

        readonly EcsFilterInject<Inc<EnemyTag, ShipComponent, InactiveTag>, Exc<DeadTag>> _enemyUnitsFilter = default;

        readonly EcsPoolInject<DisabledShipTag> _disabledShipPool = default;
        readonly EcsPoolInject<ShipComponent> _shipPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        private int _defaultActiveEncounterOnLevel = 1;

        private string Ship;

        public void Init (EcsSystems systems)
        {
            var allShips = GameObject.FindGameObjectsWithTag(nameof(Ship));

            foreach (var ship in allShips)
            {
                var shipEntity = _world.Value.NewEntity();

                ref var shipComponent = ref _shipPool.Value.Add(shipEntity);
                shipComponent.ShipArrivalMonoBehavior = ship.GetComponent<ShipArrivalMonoBehavior>();
                shipComponent.ShipArrivalMonoBehavior.SetEntity(shipEntity);
                shipComponent.ShipArrivalMonoBehavior.Init(_world);
                shipComponent.Number = shipComponent.ShipArrivalMonoBehavior.GetShipNumber();
                shipComponent.EnemyUnitsEntitys = new List<int>();

                ref var viewComponent = ref _viewPool.Value.Add(shipEntity);
                viewComponent.GameObject = ship;

                if (shipComponent.Number > _defaultActiveEncounterOnLevel)
                {
                    _disabledShipPool.Value.Add(shipEntity);
                }

                foreach (var enemyUnit in _enemyUnitsFilter.Value)
                {
                    ref var enemyUnitEncounter = ref _shipPool.Value.Get(enemyUnit);

                    if (enemyUnitEncounter.Number == shipComponent.Number)
                    {
                        shipComponent.EnemyUnitsEntitys.Add(enemyUnit);
                    }
                }
            }
        }
    }
}