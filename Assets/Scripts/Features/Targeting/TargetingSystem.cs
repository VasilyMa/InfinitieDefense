using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TargetingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EnemyTag, Targetable, ViewComponent>, Exc<InactiveTag, ShipTag, DeadTag>> _enemyFilter = default;

        readonly EcsFilterInject<Inc<TargetingEvent>> _targetingEventFilter = default;

        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<TargetingEvent> _targetingEventPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        readonly EcsSharedInject<GameState> _state;

        public void Run (EcsSystems systems)
        {
            foreach(var enemyEntity in _enemyFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(enemyEntity);

                if (targetableComponent.TargetObject)
                {
                    continue;
                }

                ref var viewComponent = ref _viewPool.Value.Get(enemyEntity);
                ref var _viewMainTowerComponent = ref _viewPool.Value.Get(_state.Value.TowersEntity[0]);

                targetableComponent.TargetEntity = _state.Value.TowersEntity[0];
                targetableComponent.TargetObject = _viewMainTowerComponent.GameObject;
            }

            foreach (var entity in _targetingEventFilter.Value)
            {
                ref var targetingEvent = ref _targetingEventPool.Value.Get(entity);

                ref var targetableComponent = ref _targetablePool.Value.Get(targetingEvent.TargetingEntity);

                if (targetableComponent.TargetObject)
                {
                    ref var oldTargetWeightComponent = ref _targetWeightPool.Value.Get(targetableComponent.TargetEntity);
                    ref var newTargetWeightComponent = ref _targetWeightPool.Value.Get(targetingEvent.TargetEntity);
                    if (oldTargetWeightComponent.Value >= newTargetWeightComponent.Value)
                    {
                        continue;
                    }
                }

                ref var targetViewComponent = ref _viewPool.Value.Get(targetingEvent.TargetEntity);
                targetableComponent.TargetEntity = targetingEvent.TargetEntity;
                targetableComponent.TargetObject = targetViewComponent.GameObject;
            }
        }
    }
}