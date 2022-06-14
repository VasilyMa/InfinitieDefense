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
            interfaceComp.joystick = GameObject.Find("Joystick");
            var joystick = interfaceComp.joystick.transform.GetChild(0).gameObject;
            
            var handler = joystick.transform.GetChild(0).gameObject;
            var handColor = handler.GetComponent<Image>().enabled = false;
            var joyColor = joystick.GetComponent<Image>().enabled = false;

            }
    }
}