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

        readonly EcsSharedInject<GameState> _state;

        private int _defaultActiveEncounterOnLevel = 1;

        public void Init (EcsSystems systems)
        {
            //var allShips = GameObject.FindGameObjectsWithTag(nameof(Ship));
            var allShips = GameObject.FindObjectsOfType<ShipArrivalMonoBehavior>();

            foreach (var ship in allShips)
            {
                var shipEntity = _world.Value.NewEntity();

                _world.Value.GetPool<EnemyTag>().Add(shipEntity);

                _world.Value.GetPool<ShipTag>().Add(shipEntity);

                ref var targetableComponent = ref _world.Value.GetPool<Targetable>().Add(shipEntity);

                ref var _viewMainTowerComponent = ref _viewPool.Value.Get(_state.Value.EntityMainTower);

                targetableComponent.TargetEntity = _state.Value.EntityMainTower;
                targetableComponent.TargetObject = _viewMainTowerComponent.GameObject;

                ref var movableComponent = ref _world.Value.GetPool<Movable>().Add(shipEntity);
                movableComponent.Speed = 10f;

                ref var shipComponent = ref _shipPool.Value.Add(shipEntity);
                shipComponent.ShipArrivalMonoBehavior = ship.GetComponent<ShipArrivalMonoBehavior>();
                shipComponent.ShipArrivalMonoBehavior.SetEntity(shipEntity);
                shipComponent.ShipArrivalMonoBehavior.Init(_world);
                shipComponent.Number = shipComponent.ShipArrivalMonoBehavior.GetShipNumber();
                shipComponent.EnemyUnitsEntitys = new List<int>();

                ref var viewComponent = ref _viewPool.Value.Add(shipEntity);
                viewComponent.GameObject = ship.gameObject;
                viewComponent.Rigidbody = ship.GetComponent<Rigidbody>();

                if (shipComponent.Number > _defaultActiveEncounterOnLevel)
                {
                    _disabledShipPool.Value.Add(shipEntity);
                }

                foreach (var enemyUnit in _enemyUnitsFilter.Value)
                {
                    ref var enemyUnitShip = ref _shipPool.Value.Get(enemyUnit);

                    if (enemyUnitShip.Number == shipComponent.Number)
                    {
                        shipComponent.EnemyUnitsEntitys.Add(enemyUnit);
                    }
                }
            }
        }
    }
}