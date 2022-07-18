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
                var startPos = _dropComp.StartPosition;
                var speed = _dropComp.Speed;
                foreach (var stone in _dropComp.Transform)
                {
                    stone.SetParent(null);
                    stone.position = Vector3.MoveTowards(stone.position, target, speed * Time.deltaTime);
                    
                }
                for (int i = 0; i < _dropComp.Transform.Count; i++)
                {
                    if (_dropComp.Transform[i].parent == null && _dropComp.Transform[i].position == target)
                    {

                    }
                }
            }
        }
    }
}