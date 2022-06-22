using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Unity;
namespace Client {
    sealed class OreMiningSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<OreEventComponent, OreComponent>> _filter = default;
        readonly EcsFilterInject<Inc<Player>> _filterPlayer = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var oreComp = ref _filter.Pools.Inc2.Get(entity);
                
                oreComp.amount--;
                var stone = (GameObject)GameObject.Instantiate(Resources.Load("Stone"), oreComp.prefab.transform.position, Quaternion.identity);
                var stonePos = new Vector3(stone.transform.position.x + Random.Range(-10.5f, 15.0f),
                    stone.transform.position.y,
                    stone.transform.position.z + Random.Range(-10.5f, 15.0f));
                stone.transform.position = Vector3.Lerp(stone.transform.position, stonePos, Time.deltaTime * 10f);
                if (oreComp.amount <= 0) 
                { 
                    oreComp.prefab.GetComponent<SphereCollider>().enabled = false;
                    oreComp.prefab.gameObject.SetActive(false); 
                    _filter.Pools.Inc2.Del(entity);
                    foreach (var entityPlayer in _filterPlayer.Value)
                    {
                        ref var player = ref _filterPlayer.Pools.Inc1.Get(entityPlayer);
                        player.animator.SetBool("isMining", false);
                        player.animator.SetBool("isIdle", true);
                    }
                }
                _filter.Pools.Inc1.Del(entity);
                
            }
        }
    }
}