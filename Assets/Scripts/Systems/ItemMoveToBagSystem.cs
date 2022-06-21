using Leopotam.EcsLite;
using UnityEngine;
using Leopotam.EcsLite.Di;
namespace Client {
    sealed class ItemMoveToBagSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<MoveToBagComponent>> _filter = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        public void Run (EcsSystems systems) {
            foreach(var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                ref var interComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                var x = Mathf.Lerp(filterComp.StartPosition.x, filterComp.TargetPosition.x, filterComp._t);
                var y = Mathf.Lerp(filterComp.StartPosition.y, filterComp.TargetPosition.y, filterComp._t);
                var z = Mathf.Lerp(filterComp.StartPosition.z, filterComp.TargetPosition.z, filterComp._t);

                filterComp._t += Time.deltaTime * 3f;

                filterComp.Transform.localPosition = new Vector3(x, y, z);

                if(filterComp._t >= 1)
                {
                    if(filterComp.Coin)
                    {
                        _state.Value.CoinTransformList.Add(filterComp.Transform);
                        interComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateGold();
                    }
                    else
                    {
                        foreach(var item in _state.Value.CoinTransformList)
                        {
                            item.localPosition = new Vector3(0, item.localPosition.y + 0.6f, 0);
                        }
                        _state.Value.StoneTransformList.Add(filterComp.Transform);
                        interComp.resourcePanel.GetComponent<ResourcesPanelMB>().UpdateStone();
                    }
                    filterComp.Transform.localPosition = filterComp.TargetPosition;
                    _filter.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}