using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CorpseRemoveSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<CorpseRemove>> _filterCorpse = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<CorpseRemove> _corpsePool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filterCorpse.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                ref var corpse = ref _corpsePool.Value.Get(entity);
                if (corpse.timer > 0)
                {
                    corpse.timer -= Time.deltaTime;
                }
                else if (corpse.timer < 2)
                {
                    viewComp.BodyCollider.enabled = false;
                }
                else if (corpse.timer <= 0)
                {
                    corpse.timer = 0;
                    GameObject.Destroy(viewComp.GameObject);
                    _filterCorpse.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}