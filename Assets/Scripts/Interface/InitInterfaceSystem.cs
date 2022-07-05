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
            interfaceComp.winPanel = GameObject.Find("WinPanel");
            interfaceComp.winPanel.SetActive(false);
            interfaceComp.losePanel = GameObject.Find("LosePanel");
            interfaceComp.losePanel.SetActive(false);
            interfaceComp.gamePanel = GameObject.Find("GamePanel");
            interfaceComp.gamePanel.SetActive(true);
            interfaceComp._joystick = GameObject.Find("Joystick").GetComponent<FloatingJoystick>();
            interfaceComp.progressbar = GameObject.Find("LevelProgress");
            interfaceComp.progressbar.GetComponent<ProgressBarMB>().SetMaxAmount(state.WaveStorage.GetAllEnemies());
            interfaceComp.progressbar.GetComponent<ProgressBarMB>().Init(systems.GetWorld(), systems.GetShared<GameState>());
            interfaceComp.progressbar.SetActive(false);
            var resourcePanel = interfaceComp.resourcePanel.GetComponent<ResourcesPanelMB>();
            resourcePanel.Init(systems.GetWorld(), systems.GetShared<GameState>());
            }
    }
}