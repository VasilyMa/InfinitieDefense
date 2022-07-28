using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Client {
    sealed class TutorialSystem : IEcsRunSystem {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<TutorialComponent>> _filter = default;
        readonly EcsPoolInject<ViewComponent> _player = default;
        readonly EcsPoolInject<TutorialComponent> _tutorialPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<CursorEvent> _cursorPool = default;
        readonly EcsPoolInject<CanvasUpgradeComponent> _upgradePoint = default;
        private GameObject player;
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var tutorialComp = ref _tutorialPool.Value.Get(entity);
                ref var interfaceComp = ref _interfacePool.Value.Get(_state.Value.EntityInterface);
                switch (tutorialComp.TutorialStage)
                {
                    case 0:

                        player = _player.Value.Get(_state.Value.EntityPlayer).GameObject;
                        player.transform.GetChild(5).GetComponent<BoxCollider>().enabled = false;
                        tutorialComp.HandObject.SetActive(true);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.DragToMove.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Drag to move";
                        tutorialComp.Animator.SetBool("isSwipe", true);
                        if (interfaceComp._joystick.Horizontal != 0 || interfaceComp._joystick.Vertical != 0)
                        {
                            tutorialComp.TutorialStage = 1;
                            _state.Value.Saves.TutorialStage = 1;
                            _state.Value.Saves.SaveTutorial(1);
                            tutorialComp.Animator.SetBool("isSwipe", false);
                            tutorialComp.HandObject.SetActive(false);
                            //tutorialComp.Background.SetActive(false);
                            tutorialComp.TextHolder.SetActive(false);
                            tutorialComp.DragToMove.SetActive(false);
                        }
                        break;
                    case 1:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Wait of the enemy's wave";
                        _world.Value.GetPool<CountdownWaveComponent>().Add(_world.Value.NewEntity());
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(3);
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(true);
                        tutorialComp.TutorialStage = 2;
                        _state.Value.Saves.TutorialStage = 2;
                        _state.Value.Saves.SaveTutorial(2);
                        break;
                        
                    case 3:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Pick up the coins";
                        break;
                    case 4:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the main upgrade point";
                        var cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(0f, 4.5f, -2.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 5;
                        _state.Value.Saves.TutorialStage = 5;
                        _state.Value.Saves.SaveTutorial(5);
                        ref var upgradePointComp = ref _upgradePoint.Value.Get(_state.Value.TowersEntity[0]);
                        upgradePointComp.point.SetActive(true);
                        break;
                    case 6:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the player upgrade point";
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(-4.5f, 4.5f, -6.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 7;
                        _state.Value.Saves.TutorialStage = 7;
                        _state.Value.Saves.SaveTutorial(7);
                        break;
                    case 8:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the ore";
                        player.transform.GetChild(5).GetComponent<BoxCollider>().enabled = true;
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(1.25f, 4.5f, -16.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 9;
                        _state.Value.Saves.TutorialStage = 9;
                        _state.Value.Saves.SaveTutorial(9);
                        break;
                    case 10:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(true);
                        tutorialComp.TextHolder.GetComponentInChildren<Text>().text = "Go to the tower upgrade point";
                        cursor = (GameObject)GameObject.Instantiate(Resources.Load("CursorHolder"), new Vector3(1f, 4.5f, -12.5f), Quaternion.identity);
                        tutorialComp.TutorialCursor = cursor;
                        tutorialComp.TutorialStage = 11;
                        _state.Value.Saves.TutorialStage = 11;
                        _state.Value.Saves.SaveTutorial(11);
                        break;
                    case 12:
                        tutorialComp.DragToMove.SetActive(false);
                        tutorialComp.HandObject.SetActive(false);
                        //tutorialComp.Background.SetActive(false);
                        tutorialComp.TextHolder.SetActive(false);
                        _world.Value.GetPool<CountdownWaveComponent>().Add(_world.Value.NewEntity());
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(30);
                        interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(true);
                        _tutorialPool.Value.Del(entity);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}