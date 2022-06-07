using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;
namespace Client {
    sealed class UserInputSystem : EcsUguiCallbackSystem
    {
        readonly EcsFilterInject<Inc<Player>> _player = default;

        [Preserve]
        [EcsUguiDragStartEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDownTouchListner(in EcsUguiDragStartEvent e)
        {
            Debug.Log("Down");
        }
        [Preserve]
        [EcsUguiDownEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDownListner(in EcsUguiDownEvent e)
        {
            Debug.Log("Input");
        }

        [EcsUguiDragMoveEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDragTouchListner(in EcsUguiDragMoveEvent e)
        {
            Debug.Log("Drag");
        }

        [Preserve]
        [EcsUguiDragEndEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnUpTouchListner(in EcsUguiDragEndEvent e)
        {
            Debug.Log("Up");
        }
    }
}
