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
                switch (tutorialComp.TutorialStage)
                {
                    case 0:
                        tutorialComp.HandObject.SetActive(true);
                        tutorialComp.HoleObject.SetActive(true);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Swipe to move";
                        tutorialComp.Animator.SetBool("isSwipe", true);
                        if (interfaceComp._joystick.Horizontal != 0 || interfaceComp._joystick.Vertical != 0)
                        {
                            tutorialComp.TutorialStage = 1;
                            _state.Value.Saves.TutorialStage = 1;
                            _state.Value.Saves.SaveTutorial(1);
                            tutorialComp.Animator.SetBool("isSwipe", false);
                            tutorialComp.HandObject.SetActive(false);
                            tutorialComp.HoleObject.SetActive(false);
                            tutorialComp.TextHolder.SetActive(false);
                        }
                        break;
                    case 1:
                        //ref var cursorComp = ref _cursorPool.Value.Add(_world.Value.NewEntity());
                        var cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(1.25f, 4.5f, -16.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 2;
                        _state.Value.Saves.TutorialStage = 2;
                        _state.Value.Saves.SaveTutorial(2);
                        break;
                    case 2:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the ore";
                        break;
                    case 3:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the coin";
                        break;
                    case 4:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the tower upgrade point";
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(1f, 4.5f, -12.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 5;
                        _state.Value.Saves.TutorialStage = 5;
                        _state.Value.Saves.SaveTutorial(5);
                        break;
                    case 6:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the main upgrade point";
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(0f, 4.5f, -2.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 7;
                        _state.Value.Saves.TutorialStage = 7;
                        _state.Value.Saves.SaveTutorial(7);
                        break;
                    case 8:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the player upgrade point";
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(-4.5f, 4.5f, -6.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 9;
                        _state.Value.Saves.TutorialStage = 9;
                        _state.Value.Saves.SaveTutorial(9);
                        break;
                    case 10:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Wait of the enemy's wave";
                        _world.Value.GetPool<CountdownWaveComponent>().Add(_world.Value.NewEntity());
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(10);
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(true);
                        tutorialComp.TutorialStage = 11;
                        _state.Value.Saves.TutorialStage = 11;
                        _state.Value.Saves.SaveTutorial(11);
                        break;
                    case 12:
                        tutorialComp.HandObject.SetActive(false);
                        tutorialComp.HoleObject.SetActive(false);
                        tutorialComp.TextHolder.SetActive(false);
                        _tutorialPool.Value.Del(entity);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}