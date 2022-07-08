using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckWeaponSystem : IEcsRunSystem {
        readonly EcsFilterInject<Inc<Player>, Exc<InFightTag>> _playerFilter = default;
        readonly EcsFilterInject<Inc<Player, InFightTag>> _playerInFight = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        public void Run (EcsSystems systems) {
            foreach (var entity in _playerFilter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(false);
                if (viewComp.isMining)
                {
                    viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(false);
                    viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(true);
                }
                else if (!viewComp.isMining)
                {
                    viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(false);
                }
            }
            foreach (var entity in _playerInFight.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(entity);
                viewComp.WeaponHolder.GetChild(0).gameObject.SetActive(false);
                viewComp.WeaponHolder.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}