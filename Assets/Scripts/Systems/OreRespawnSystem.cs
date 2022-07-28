using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class OreRespawnSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<OreMinedTag, OreComponent>> _filter = default;
        readonly EcsPoolInject<OreComponent> _orePool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var oreComp = ref _orePool.Value.Get(entity);
                ref var viewComponent = ref _viewPool.Value.Get(entity);

                if (oreComp.respawnTime > 0)
                    oreComp.respawnTime -= Time.deltaTime;
                else if(oreComp.respawnTime <= 0)
                {
                    oreComp.respawnTime = 0;
                    oreComp.prefab.SetActive(true);
                    for (int i = 0; i < oreComp.OreParts.Length; i++)
                    {
                        oreComp.OreParts[i].SetActive(true);
                    }
                    oreComp.prefab.GetComponent<SphereCollider>().enabled = true;
                    oreComp.CurrentAmount = 4;
                    viewComponent.Animator.SetTrigger("Extract");

                    _filter.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}