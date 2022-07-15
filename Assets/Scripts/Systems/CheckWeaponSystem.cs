using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client {
    sealed class CheckWeaponSystem : IEcsRunSystem {
        /*readonly EcsFilterInject<Inc<Player>, Exc<InFightTag>> _playerFilter = default;
        readonly EcsFilterInject<Inc<Player, InFightTag>> _playerInFight = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;*/

        readonly EcsFilterInject<Inc<Player, ContextToolComponent>> _playerFilter = default;
        readonly EcsPoolInject<ContextToolComponent> _contextToolPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var playerEntity in _playerFilter.Value)
            {
                ref var contextToolComponent = ref _contextToolPool.Value.Get(playerEntity);

                switch (contextToolComponent.ActiveTool)
                {
                    case ContextToolComponent.Tool.pickaxe:
                        if (!contextToolComponent.Pickaxe.activeSelf)
                        {
                            contextToolComponent.Pickaxe.SetActive(true);
                            contextToolComponent.Sword.SetActive(false);
                            contextToolComponent.Bow.SetActive(false);
                        }
                        break;

                    case ContextToolComponent.Tool.sword:
                        if (!contextToolComponent.Sword.activeSelf)
                        {
                            contextToolComponent.Pickaxe.SetActive(false);
                            contextToolComponent.Sword.SetActive(true);
                            contextToolComponent.Bow.SetActive(false);
                        }
                        break;

                    case ContextToolComponent.Tool.bow:
                        if (!contextToolComponent.Bow.activeSelf)
                        {
                            contextToolComponent.Pickaxe.SetActive(false);
                            contextToolComponent.Sword.SetActive(false);
                            contextToolComponent.Bow.SetActive(true);
                        }
                        break;

                    case ContextToolComponent.Tool.empty:
                        contextToolComponent.Pickaxe.SetActive(false);
                        contextToolComponent.Sword.SetActive(false);
                        contextToolComponent.Bow.SetActive(false);
                        break;
                }
            }

            /*foreach (var entity in _playerFilter.Value)
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
            }*/
        }
    }
}