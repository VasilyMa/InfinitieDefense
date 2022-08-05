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
                ref var corpse = ref _corpsePool.Value.Get(entity);
                ref var viewComp = ref _viewPool.Value.Get(corpse.Entity);
                if (corpse.timer > 0)
                {
                    corpse.timer -= Time.deltaTime;
                    if (corpse.timer < 2)
                    {
                        viewComp.BodyCollider.enabled = false;
                    }
                }
                if (corpse.timer <= 0)
                {
                    corpse.timer = 0;
                    viewComp.GameObject.SetActive(false);
                    _filterCorpse.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}