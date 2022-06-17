using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class RaycastUserSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsWorldInject _world = default;
        readonly EcsFilterInject<Inc<Player>> _filter = default;
        readonly EcsFilterInject<Inc<OreComponent>> _oreFilter = default;
        readonly EcsFilterInject<Inc<ViewComponent, EnemyTag>> _enemyfilter = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;
        readonly EcsPoolInject<RealoadComponent> _reloadPool = default;
        private float distanceRay = 30f;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var _player = ref _filter.Pools.Inc1.Get(entity);
                ref var cooldown = ref _cooldownPool.Value.Get(entity);
                
                Ray ray = new Ray(_player.Transform.position + new Vector3(0, -0.5f, 0), _player.Transform.forward);
                Debug.DrawRay(_player.Transform.position, _player.Transform.forward * distanceRay);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, distanceRay))
                {
                    var dis = Vector3.Distance(_player.Transform.position, hitInfo.transform.position);
                    if (hitInfo.collider.gameObject.CompareTag("Ore"))
                    {
                        //Debug.Log($"Target is {hitInfo.collider.name}, {dis}");
                        if (cooldown.currentValue == 0 && dis <= 2)
                        {
                            cooldown.currentValue = cooldown.maxValue;
                            _player.animator.SetBool("isIdle", false);
                            _player.animator.SetTrigger("Mining");
                            _reloadPool.Value.Add(entity);
                            foreach (var oreEntity in _oreFilter.Value)
                            {
                                if (hitInfo.collider.gameObject == _oreFilter.Pools.Inc1.Get(oreEntity).prefab)
                                {
                                    //_world.Value.GetPool<OreEventComponent>().Add(oreEntity);
                                    _player.playerMB.InitMiningEvent(oreEntity, hitInfo.collider.gameObject);
                                }
                            }
                            Debug.Log($"Mining!");
                        }
                        else
                        {
                            _player.animator.SetBool("isIdle", true);
                        }
                    }
                    else if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                    {
                        //Debug.Log($"Target is {hitInfo.collider.name}, {dis}");
                        if (cooldown.currentValue == 0 && dis <= 5)
                        {
                            cooldown.currentValue = cooldown.maxValue;
                            _player.animator.SetBool("isIdle", false);
                            _player.animator.SetTrigger("Attack");
                            _reloadPool.Value.Add(entity);
                            foreach (var enemyEntity in _enemyfilter.Value)
                            {
                                if (hitInfo.collider.gameObject == _enemyfilter.Pools.Inc1.Get(enemyEntity).GameObject)
                                {
                                    _player.playerMB.InitCombatEvent(enemyEntity, hitInfo.collider.gameObject);
                                    _player.AttackMonoBehaviour.SetTargetInfo(enemyEntity, hitInfo.collider.gameObject);
                                }
                            }
                            Debug.Log("Attack!");
                            //to do attack event
                        }
                        else
                        {
                            _player.animator.SetBool("isIdle", true);
                        }
                    }
                }
            }
            
        }
    }
}