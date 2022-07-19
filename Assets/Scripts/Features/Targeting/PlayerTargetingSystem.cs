using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    sealed class PlayerTargetingSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Targetable, Player>, Exc<DeadTag>> _playerFilter = default;

        readonly EcsPoolInject<Targetable> _targetablePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<DeadTag> _deadPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                ref var targetableComponent = ref _targetablePool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                if (targetableComponent.TargetEntity > 0)
                {
                    if (_deadPool.Value.Has(targetableComponent.TargetEntity))
                    {
                        targetableComponent.TargetEntity = -1;
                        targetableComponent.TargetObject = null;
                        viewComponent.EcsInfoMB.ResetTarget();
                    }
                }

                if (targetableComponent.TargetEntity > -1 && targetableComponent.TargetObject == null)
                {
                    targetableComponent.TargetObject = _viewPool.Value.Get(targetableComponent.TargetEntity).GameObject;
                }

                if (targetableComponent.AllEntityInDamageZone.Count == 0)
                {
                    targetableComponent.TargetEntity = -1;
                    targetableComponent.TargetObject = null;
                    viewComponent.EcsInfoMB.ResetTarget();
                    continue;
                }

                List<int> allDeadEntitys = new List<int>();

                bool targetInDamageZone = false;

                foreach (var entityInDamageZone in targetableComponent.AllEntityInDamageZone)
                {
                    if (_deadPool.Value.Has(entityInDamageZone))
                    {
                        allDeadEntitys.Add(entityInDamageZone);
                        Debug.Log("Энтити находилась в пуле мертвых");
                    }

                    if (_viewPool.Value.Get(entityInDamageZone).GameObject == targetableComponent.TargetObject)
                    {
                        targetInDamageZone = true;
                    }
                }

                foreach (var deadEntity in allDeadEntitys)
                {
                    targetableComponent.AllEntityInDamageZone.Remove(deadEntity);
                }

                if (targetableComponent.AllEntityInDamageZone.Count == 0)
                {
                    targetableComponent.TargetEntity = -1;
                    targetableComponent.TargetObject = null;
                    viewComponent.EcsInfoMB.ResetTarget();
                    continue;
                }

                if (!targetInDamageZone)
                {
                    targetableComponent.TargetEntity = targetableComponent.AllEntityInDamageZone[0];
                    targetableComponent.TargetObject = _viewPool.Value.Get(targetableComponent.TargetEntity).GameObject;
                    viewComponent.EcsInfoMB.SetTarget(targetableComponent.TargetEntity, targetableComponent.TargetObject);
                }

                if (targetableComponent.TargetEntity < 1)
                {
                    targetableComponent.TargetEntity = targetableComponent.AllEntityInDamageZone[0];
                    targetableComponent.TargetObject = _viewPool.Value.Get(targetableComponent.TargetEntity).GameObject;
                    viewComponent.EcsInfoMB.SetTarget(targetableComponent.TargetEntity, targetableComponent.TargetObject);
                    Debug.Log("Попытались записать новую цель");
                }
            }
        }
    }
}