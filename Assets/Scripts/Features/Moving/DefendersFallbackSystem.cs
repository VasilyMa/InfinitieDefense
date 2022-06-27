using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class DefendersFallbackSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<UnitTag, ViewComponent, Movable, Targetable, DefenderComponent>, Exc<InFightTag, InactiveTag, DeadTag, IsNotMovableTag>> _allUnitsFilter = default;

        readonly EcsPoolInject<Movable> _movablePool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<DefenderComponent> _defenderPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var unitEntity in _allUnitsFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(unitEntity);
                ref var defenderComponent = ref _defenderPool.Value.Get(unitEntity);
                ref var movableComponent = ref _movablePool.Value.Get(unitEntity);
                ref var viewComponent = ref _viewPool.Value.Get(unitEntity);

                if (targetableComponent.TargetEntity != -1)
                {
                    continue;
                }

                float distanceToSpawn = Mathf.Sqrt((defenderComponent.Position - viewComponent.Transform.position).sqrMagnitude);

                if (distanceToSpawn > 1)
                {
                    movableComponent.Destination = defenderComponent.Position;
                }
                else
                {
                    //movableComponent.Destination = null;
                }
            }
        }
    }
}