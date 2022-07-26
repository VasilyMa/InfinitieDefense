using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class TutorialSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _filter = default;
        readonly EcsPoolInject<TutorialComponent> _tutorialPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<CursorEvent> _cursorPool = default;
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var tutorialComp = ref _tutorialPool.Value.Get(entity);
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                if (tutorialComp.TutorialStage == 0)
                {
                    tutorialComp.HandObject.SetActive(true);
                    tutorialComp.HoleObject.SetActive(true);
                    tutorialComp.TextHolder.SetActive(true);
                    tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Swipe to move";
                    tutorialComp.Animator.SetBool("isSwipe", true);
                    if (interfaceComp._joystick.Horizontal != 0 || interfaceComp._joystick.Vertical != 0)
                    {
                        tutorialComp.TutorialStage = 1;
                        _state.Value.Saves.SaveTutorial(1);
                        tutorialComp.Animator.SetBool("isSwipe", false);
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(false);
                    }
                }
                if (tutorialComp.TutorialStage == 1)
                {
                    ref var cursorComp = ref _cursorPool.Value.Add(_world.Value.NewEntity());

                }
            }
        }
    }
}