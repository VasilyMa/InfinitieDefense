using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class TargetingEventSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TargetingEvent>> _targetingEventFilter = default;

        readonly EcsPoolInject<TargetingEvent> _targetingEventPool = default;
        readonly EcsPoolInject<TargetWeightComponent> _targetWeightPool = default;
        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;

        readonly EcsSharedInject<GameState> _state;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _targetingEventFilter.Value)
            {
                Debug.Log("Мы зашли в Таргетинг эвент");

                ref var targetingEvent = ref _targetingEventPool.Value.Get(entity);

                ref var targetableComponent = ref _targetablePool.Value.Get(targetingEvent.TargetingEntity);

                if (_deadPool.Value.Has(targetingEvent.TargetEntity))
                {
                    Debug.Log("Цель, нанесшая нам урон оказалась мёртвой");
                    continue;
                }

                if (targetableComponent.TargetEntity == -1)
                {
                    targetableComponent.TargetEntity = targetingEvent.TargetEntity;
                    targetableComponent.TargetObject = _viewPool.Value.Get(targetingEvent.TargetEntity).GameObject;
                    _viewPool.Value.Get(targetingEvent.TargetingEntity).EcsInfoMB.SetTarget(targetableComponent.TargetEntity, targetableComponent.TargetObject);
                    Debug.Log("У нас небыло цели, поэтому мы сразу перезаписали её на новую, нанёсшую нам урон");
                    continue;
                }
                
                ref var oldTargetWeightComponent = ref _targetWeightPool.Value.Get(targetableComponent.TargetEntity);
                ref var newTargetWeightComponent = ref _targetWeightPool.Value.Get(targetingEvent.TargetEntity);

                if (oldTargetWeightComponent.Value < newTargetWeightComponent.Value)
                {
                    targetableComponent.TargetEntity = targetingEvent.TargetEntity;
                    targetableComponent.TargetObject = _viewPool.Value.Get(targetingEvent.TargetEntity).GameObject;

                    _viewPool.Value.Get(targetingEvent.TargetingEntity).EcsInfoMB.SetTarget(targetableComponent.TargetEntity, targetableComponent.TargetObject);
                    Debug.Log("Вес новой цели ("+ newTargetWeightComponent.Value + ") оказался больше, чем старой(" + oldTargetWeightComponent.Value + "). Цель перезаписана");
                    continue;
                }
                Debug.Log("Вес старой цели (" + oldTargetWeightComponent.Value + ") оказался больше, чем новой(" + newTargetWeightComponent.Value + ")");
            }
        }
    }
}