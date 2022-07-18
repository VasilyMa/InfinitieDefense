using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class OnOffUnitAttack : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<UnitTag, Targetable>, Exc<InactiveTag, DeadTag>> _inFightFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<InFightTag> _inFightPool = default;
        readonly EcsPoolInject<IsNotMovableTag> _isNotMovablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<InMiningTag> _miningPool  = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _inFightFilter.Value)
            {
                _miningPool.Value.Del(_state.Value.EntityPlayer);
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                bool targetObjectInDamageZone = false;

                foreach (var entityInDamageZone in targetableComponent.AllEntityInDamageZone)
                {
                    if (_viewPool.Value.Get(entityInDamageZone).GameObject == targetableComponent.TargetObject)
                    {
                        targetObjectInDamageZone = true;
                    }
                }

                if (targetObjectInDamageZone)
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