using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;


namespace Client {
    sealed class LevelPopupSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<LevelUpEvent>> _levelUpFilter = default;
        readonly EcsPoolInject<LevelUpEvent> _levelUpPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _levelUpFilter.Value)
            {
                ref var levelComp = ref _levelUpPool.Value.Get(entity);
                levelComp.LevelPopUp.transform.position = Vector3.MoveTowards(levelComp.LevelPopUp.transform.position, levelComp.target, 4 * Time.deltaTime);
                levelComp.TimeOut -= Time.deltaTime;
                if (levelComp.TimeOut <= 0)
                {
                    levelComp.TimeOut = 0;
                    levelComp.LevelPopUp.SetActive(false);
                    _levelUpFilter.Pools.Inc1.Del(entity);
                }
            }
        }
    }
}