using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemyTargetingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<EnemyTag, Targetable, ViewComponent>, Exc<InactiveTag, ShipTag, DeadTag>> _enemyFilter = default;

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

                // ubrat' proverku v collader unita
                if (viewComponent.AttackMonoBehaviour)
                {
                    viewComponent.AttackMonoBehaviour.SetTargetInfo(targetableComponent.TargetEntity, targetableComponent.TargetObject);
                }
            }
        }
    }
}