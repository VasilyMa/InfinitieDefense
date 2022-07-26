using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections.Generic;
using UnityEngine;
namespace Client {
    sealed class CursorSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CursorEvent>> _cursorFilter = default;
        readonly EcsFilterInject<Inc<OreComponent>> _orePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        private List<float> allDistance = new List<float>();
        private float min = float.MaxValue;
        public void Run (EcsSystems systems) {
            foreach (var entity in _cursorFilter.Value)
            {
                foreach (var ore in _orePool.Value)
                {
                    ref var viewComp = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                    ref var oreComp = ref _orePool.Pools.Inc1.Get(ore);

                    var dis = Vector3.Distance(viewComp.GameObject.transform.position, oreComp.prefab.transform.position);
                    allDistance.Add(dis);
                }
                for (int i = 0; i < allDistance.Count; i++)
                {
                    if (min > allDistance[i])
                    {
                        min = allDistance[i];
                    }
                }
                /*foreach (var item in _orePool.Value)
                {
                    ref var viewComp = ref _viewPool.Value.Get(_state.Value.EntityPlayer);
                    ref var oreComp = ref _orePool.Pools.Inc1.Get(item);
                    var dis = Vector3.Distance(viewComp.GameObject.transform.position, oreComp.prefab.transform.position);
                    if (dis <= min)
                        oreComp.cursor.SetActive(true);
                        
                }*/
            }
        }
    }
}