using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class OreMoveSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<OreMoveEvent>> _filter = default;
        public void Run (EcsSystems systems) 
        {
            foreach (var entity in _filter.Value)
            {
                ref var filterComp = ref _filter.Pools.Inc1.Get(entity);
                var target = filterComp.TargetPosition;
                var transform = filterComp.stone.transform;
                var speed = filterComp.Speed;
                ref var outComp = ref filterComp.outTime;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                outComp -= Time.deltaTime;
                if (outComp <= 0)
                {
                    _world.Value.DelEntity(entity);
                }
            }
        }
    }
}