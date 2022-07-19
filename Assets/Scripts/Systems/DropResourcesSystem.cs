using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class DropResourcesSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<DropByDieComponent>> _filter = default;
        readonly EcsPoolInject<DropByDieComponent> _dropPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var _dropComp = ref _dropPool.Value.Get(entity);
                var target = _dropComp.TargetPosition;
                var speed = _dropComp.Speed;
                var stone = _dropComp.Object;
                if(stone.transform.parent != null)
                    stone.transform.SetParent(null);
                stone.transform.position = Vector3.MoveTowards(stone.transform.position, target, speed * Time.deltaTime);
                speed += 0.5f;
                //stone.transform.position = new Vector3(target.x, target.y, target.z);
                if (stone.transform.position == target)
                    _filter.Pools.Inc1.Del(entity);
            }
        }
    }
}