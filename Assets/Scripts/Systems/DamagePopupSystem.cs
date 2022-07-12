using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.UI;
using UnityEngine;
namespace Client {
    sealed class DamagePopupSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<DamagePopupEvent>> _filterDamage = default;
        readonly EcsPoolInject<DamagePopupEvent> _damageEventPool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _filterDamage.Value)
            {
                ref var damageComp = ref _damageEventPool.Value.Get(entity);
                ref var cameraComp = ref _cameraPool.Value.Get(_state.Value.EntityCamera);
                damageComp.DamageObject.SetActive(true);
                damageComp.DamageObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = damageComp.DamageAmount.ToString();
                damageComp.DamageObject.transform.position = Vector3.MoveTowards(damageComp.DamageObject.transform.position, damageComp.target, 2 * Time.deltaTime);
                damageComp.DamageObject.transform.LookAt(damageComp.DamageObject.transform.position + cameraComp.CameraTransform.forward);
                if (damageComp.timeOut > 0)
                    damageComp.timeOut -= Time.deltaTime;
                else if (damageComp.timeOut <= 0)
                {
                    damageComp.timeOut = 0;
                    damageComp.DamageObject.SetActive(false);
                    _filterDamage.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}