using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TargetingEventSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TargetingEvent>> _targetingEventFilter = default;

        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<TargetingEvent> _targetingEventPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;
        readonly EcsPoolInject<EnemyTag> _enemyPool = default;

        readonly EcsSharedInject<GameState> _state;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _targetingEventFilter.Value)
            {
                ref var targetingEvent = ref _targetingEventPool.Value.Get(entity);

                ref var targetableComponent = ref _targetablePool.Value.Get(targetingEvent.TargetingEntity);

                if (targetingEvent.TargetEntity == -1 || targetableComponent.TargetObject == null)
                {
                    targetableComponent.TargetEntity = targetingEvent.TargetEntity;
                    targetableComponent.TargetObject = null;
                    continue;
                }

                if (_deadPool.Value.Has(targetingEvent.TargetEntity))
                {
                    targetableComponent.TargetEntity = -1;
                    targetableComponent.TargetObject = null;
                    continue;
                }

                if (!_deadPool.Value.Has(targetableComponent.TargetEntity))
                {
                    ref var oldTargetWeightComponent = ref _targetWeightPool.Value.Get(targetableComponent.TargetEntity);
                    ref var newTargetWeightComponent = ref _targetWeightPool.Value.Get(targetingEvent.TargetEntity);

                    if (oldTargetWeightComponent.Value >= newTargetWeightComponent.Value)
                    {
                        continue;
                    }
                }

                ref var viewComponent = ref _viewPool.Value.Get(targetingEvent.TargetingEntity);
                ref var targetViewComponent = ref _viewPool.Value.Get(targetingEvent.TargetEntity);
                targetableComponent.TargetEntity = targetingEvent.TargetEntity;
                targetableComponent.TargetObject = targetViewComponent.GameObject;

                viewComponent.EcsInfoMB.SetTarget(targetableComponent.TargetEntity, targetableComponent.TargetObject);
            }
        }
    }
}