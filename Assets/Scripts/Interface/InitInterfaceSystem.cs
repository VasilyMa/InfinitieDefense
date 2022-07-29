using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.UI;
namespace Client {
    sealed class InitInterfaceSystem : IEcsInitSystem {
        
        public void Init (EcsSystems systems) {
            var world = systems.GetWorld();
            var state = systems.GetShared<GameState>();
            int entity = world.NewEntity();
            state.EntityInterface = entity;

            ref var interfaceComp = ref world.GetPool<InterfaceComponent>().Add(entity);
            
            
            interfaceComp.resourcePanel = GameObject.Find("ResourcesPanel");
            interfaceComp.resourcePanelMB = interfaceComp.resourcePanel.GetComponent<ResourcesPanelMB>();
            interfaceComp.winPanel = GameObject.Find("WinPanel");
            interfaceComp.winPanel.SetActive(false);
            interfaceComp.losePanel = GameObject.Find("LosePanel");
            interfaceComp.losePanel.SetActive(false);
            interfaceComp.waveCounter = GameObject.Find("WaveCounter");
            var joystick = GameObject.Find("Joystick");
            interfaceComp._joystick = joystick.GetComponent<FloatingJoystick>();
            interfaceComp._joystickPoint = joystick.transform.GetChild(0).transform.GetChild(0).transform;
            interfaceComp._joysticKCenter = joystick.transform.GetChild(0).transform;
            interfaceComp.gamePanel = GameObject.Find("GamePanel");
            interfaceComp.gamePanel.SetActive(true);
            interfaceComp.progressbar = GameObject.Find("LevelProgress");
            interfaceComp.progressbar.GetComponent<ProgressBarMB>().SetMaxAmount(state.WaveStorage.GetAllEnemies());
            interfaceComp.progressbar.GetComponent<ProgressBarMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            interfaceComp.progressbar.SetActive(true);
            interfaceComp.waveCounter.GetComponent<WaveCounterMB>().SetMaxAmount(state.WaveStorage.Waves.Count);
            interfaceComp.waveCounter.GetComponent<WaveCounterMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            interfaceComp.countdownWave = GameObject.Find("WaveTimer");
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(0);
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(false);
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());

            //world.GetPool<CountdownWaveComponent>().Add(world.NewEntity());
            var resourcePanel = interfaceComp.resourcePanelMB;
            resourcePanel.Init(systems.GetWorld(), systems.GetShared<GameState>());
            resourcePanel.DisableGoldPanel();
            resourcePanel.DisableStonePanel();
            //tutorial there
            ref var tutorialComp = ref world.GetPool<TutorialComponent>().Add(world.NewEntity());
            tutorialComp.Tutorial = GameObject.Find("Tutorial");
            tutorialComp.TextHolder = GameObject.Find("TextHolder");
            tutorialComp.HandObject = GameObject.Find("Hand");
            //tutorialComp.Background = GameObject.Find("Background");
            tutorialComp.DragToMove = GameObject.Find("DragToMove");
            tutorialComp.TutorialStage = state.Saves.TutorialStage;
            tutorialComp.Animator = tutorialComp.Tutorial.GetComponent<Animator>();
            if (state.Saves.TutorialStage == 12)
            {
                tutorialComp.TextHolder.SetActive(false);
                tutorialComp.HandObject.SetActive(false);
                //tutorialComp.Background.SetActive(false);
                tutorialComp.DragToMove.SetActive(false);
                state.EnemiesWave = -1;
                ref var countdown = ref world.GetPool<CountdownWaveComponent>().Add(world.NewEntity());
                interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(30);
                interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(true);
            }
        }
    }
}