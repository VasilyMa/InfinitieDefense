using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class JoinToFightSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable>, Exc<InactiveTag, DeadTag, ShipTag, TowerTag>> _inFightFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<InFightTag> _inFightPool = default;
        readonly EcsPoolInject<IsNotMovableTag> _isNotMovablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<Player> _playerPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _inFightFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                if (targetableComponent.AllEntityInDamageZone.Count == 0)
                {
                    if (_inFightPool.Value.Has(entity)) _inFightPool.Value.Del(entity);
                    if (_isNotMovablePool.Value.Has(entity)) _isNotMovablePool.Value.Del(entity);
                    viewComponent.Animator.SetBool("Attack", false);
                    continue;
                }

                bool targetInDamageZone = false;

                if (_playerPool.Value.Has(entity))
                {
                    targetInDamageZone = true;
                }

                foreach (var entityInDamageZone in targetableComponent.AllEntityInDamageZone)
                {
                    if (entityInDamageZone == targetableComponent.TargetEntity)
                    {
                        targetInDamageZone = true;
                    }
                }

                if (targetInDamageZone)
                {
                    if (!_inFightPool.Value.Has(entity)) _inFightPool.Value.Add(entity);
                    if (!_isNotMovablePool.Value.Has(entity)) _isNotMovablePool.Value.Add(entity);
                    viewComponent.Animator.SetBool("Attack", true);
                }
                else
                {
                    if (_inFightPool.Value.Has(entity)) _inFightPool.Value.Del(entity);
                    if (_isNotMovablePool.Value.Has(entity)) _isNotMovablePool.Value.Del(entity);
                    viewComponent.Animator.SetBool("Attack", false);
                }
            }
        }
    }
}