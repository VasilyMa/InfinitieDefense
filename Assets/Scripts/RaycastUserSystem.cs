using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
namespace Client {
    sealed class RaycastUserSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<Player>> _filter = default;
        readonly EcsPoolInject<CooldownComponent> _cooldownPool = default;
        readonly EcsPoolInject<StoneMiningEvent> _miningPool = default;
        readonly EcsPoolInject<RealoadComponent> _reloadPool = default;
        private float distanceRay = 30f;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filter.Value)
            {
                ref var _player = ref _filter.Pools.Inc1.Get(entity);
                ref var cooldown = ref _cooldownPool.Value.Get(entity);
                Ray ray = new Ray(_player.Transform.position, _player.Transform.forward);
                Debug.DrawRay(_player.Transform.position, _player.Transform.forward * distanceRay);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, distanceRay))
                {
                    var dis = Vector3.Distance(_player.Transform.position, hitInfo.transform.position);
                    if (hitInfo.collider.gameObject.CompareTag("Stone"))
                    {
                        Debug.Log($"Target is {hitInfo.collider.name}, {dis}");
                        if (cooldown.currentValue == 0 && dis <= 2)
                        {
                            Debug.Log("Mining!");
                            cooldown.currentValue = cooldown.maxValue;
                            _player.animator.SetBool("isMining", true);
                            _reloadPool.Value.Add(entity);
                            _miningPool.Value.Add(entity);
                        }
                    }
                    if (hitInfo.collider.gameObject.CompareTag("Enemie"))
                    {
                        Debug.Log($"Target is {hitInfo.collider.name}, {dis}");
                        if (cooldown.currentValue == 0 && dis <= 2)
                        {
                            Debug.Log("Attack!");
                            cooldown.currentValue= cooldown.maxValue;
                            _player.animator.SetBool("isAttack", true);
                            _reloadPool.Value.Add(entity);
                            //to do attack event
                        }
                    }
                    _player.animator.SetBool("isMining", false);
                    _player.animator.SetBool("isAttack", false);
                }
            }
            
        }
    }
}