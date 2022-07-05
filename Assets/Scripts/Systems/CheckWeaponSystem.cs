using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckWeaponSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<Player>> _playerFilter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _playerFilter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                if (viewComp.isMining)
                {
                    viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(false);
                    viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(true);
                }
                else if (!viewComp.isMining)
                {
                    viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(false);
                }
                if (viewComp.isFight)
                {
                    viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(false);
                    viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(true);
                }
                else if (!viewComp.isFight)
                {
                    viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }
}