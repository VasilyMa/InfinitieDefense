using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Client
{
    sealed class LookingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ViewComponent, Targetable>> _entitysFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _entitysFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);

                if (!targetableComponent.TargetObject)
                {
                    continue;
                }

                ref var viewComponent = ref _viewPool.Value.Get(entity);

                Vector3 flatTargetPosition = new Vector3(targetableComponent.TargetObject.transform.position.x,
                                                            viewComponent.GameObject.transform.position.y,
                                                            targetableComponent.TargetObject.transform.position.z);

                viewComponent.GameObject.transform.LookAt(flatTargetPosition);
            }
        }
    }
}