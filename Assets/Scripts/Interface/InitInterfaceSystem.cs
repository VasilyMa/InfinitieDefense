using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.UI;
namespace Client {
    sealed class InitInterfaceSystem : IEcsInitSystem {

        public void Init(EcsSystems systems) {
            var world = systems.GetWorld();
            var state = systems.GetShared<GameState>();
            int entity = world.NewEntity();
            state.EntityInterface = entity;

            ref var interfaceComp = ref world.GetPool<InterfaceComponent>().Add(entity);

            interfaceComp.continueButton = GameObject.Find("Continue");
            interfaceComp.continueButton.SetActive(false);
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
            //interfaceComp.progressbar = GameObject.Find("LevelProgress");
            //interfaceComp.progressbar.GetComponent<ProgressBarMB>().SetMaxAmount(state.WaveStorage.GetAllEnemies());
            //interfaceComp.progressbar.GetComponent<ProgressBarMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            //interfaceComp.progressbar.SetActive(true);
            //interfaceComp.waveCounter.GetComponent<WaveCounterMB>().SetMaxAmount(state.WaveStorage.Waves.Count);
            //interfaceComp.waveCounter.GetComponent<WaveCounterMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            interfaceComp.countdownWave = GameObject.Find("WaveTimer");
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(0);
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SwitcherTurn(false);
            interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            interfaceComp._waveCounter = GameObject.Find("Counter").transform;
            interfaceComp._waveCounter.GetComponent<CounterMB>().points = new System.Collections.Generic.List<Image>();
            interfaceComp._waveCounter.GetComponent<CounterMB>().sliders = new System.Collections.Generic.List<Slider>();
            for (int i = 0; i < state.WaveStorage.Waves.Count + 1; i++)
            {
                var point = GameObject.Instantiate(state.InterfaceStorage.Point, interfaceComp._waveCounter);
                interfaceComp._waveCounter.GetComponent<CounterMB>().points.Add(point.GetComponent<Image>());
                if (i < state.WaveStorage.Waves.Count)
                {
                    var slider = GameObject.Instantiate(state.InterfaceStorage.Slider, interfaceComp._waveCounter);
                    slider.GetComponent<Slider>().maxValue = 1;
                    slider.GetComponent<Slider>().value = 1;
                    interfaceComp._waveCounter.GetComponent<CounterMB>().sliders.Add(slider.GetComponent<Slider>());
                    slider.GetComponent<RectTransform>().sizeDelta = new Vector2(slider.GetComponent<RectTransform>().sizeDelta.x / state.WaveStorage.Waves.Count * 2, 60);
                }
            }
            //interfaceComp._waveCounter.localScale = interfaceComp._waveCounter.localScale / state.WaveStorage.Waves.Count;
            interfaceComp._waveCounter.GetComponent<CounterMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            
            //world.GetPool<CountdownWaveComponent>().Add(world.NewEntity());
            var resourcePanel = interfaceComp.resourcePanelMB;
            resourcePanel.Init(systems.GetWorld(), systems.GetShared<GameState>());
            resourcePanel.DisableGoldPanel();
            resourcePanel.DisableStonePanel();
            if (state.CoinCount > 0)
            {
                resourcePanel.EnableGoldPanel();
                resourcePanel.UpdateGold();
            }

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
                /*ref var countdown = ref world.GetPool<CountdownWaveComponent>().Add(world.NewEntity());
                interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetTimer(state.TimeToNextWave);
                interfaceComp.countdownWave.GetComponent<CountdownWaveMB>().SetText("Next wave!");*/
            }
        }
    }
}