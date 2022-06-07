using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace Client {
    sealed class EcsStartup : MonoBehaviour {
        [SerializeField] EcsUguiEmitter _uguiEmitter;
        EcsSystems _systems;
        EcsWorld _world;
        void Start () {
            _world = new EcsWorld();

            // register your shared data here, for example:
            // var shared = new Shared ();
            // systems = new EcsSystems (new EcsWorld (), shared);
            _systems = new EcsSystems (_world);
            _systems
                .Add(new PlayerInitSystem())
                .Add(new UserInputSystem())
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())

                // register additional worlds here, for example:
                .AddWorld(new EcsWorld(), Idents.Worlds.Events)
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Idents.Worlds.Events))
#endif
                .InjectUgui(_uguiEmitter, Idents.Worlds.Events)
                .Inject()
                .Init ();
        }

        void Update () {
            _systems?.Run ();
        }

        void OnDestroy () {
            if (_systems != null) {
                _systems.Destroy ();
                // add here cleanup for custom worlds, for example:
                // _systems.GetWorld ("events").Destroy ();
                _systems.GetWorld ().Destroy ();
                _systems = null;
            }
        }
    }
}