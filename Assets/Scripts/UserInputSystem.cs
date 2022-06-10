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
        void OnDownTouchListner(in EcsUguiDragStartEvent evt)
        {
            Debug.Log("Down", evt.Sender);
        }
        [Preserve]
        [EcsUguiDownEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDownListner(in EcsUguiDownEvent evt)
        {
            Debug.Log("Input", evt.Sender);
        }
        [Preserve]
        [EcsUguiDragMoveEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDragTouchListner(in EcsUguiDragMoveEvent evt)
        {
            Debug.Log("Drag", evt.Sender);
        }
        [Preserve]
        [EcsUguiDragEndEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnUpTouchListner(in EcsUguiDragEndEvent evt)
        {
            Debug.Log("Up", evt.Sender);
        }
        [Preserve] 
        [EcsUguiClickEvent("AnyButton")]
        void OnAnyClick(in EcsUguiClickEvent evt)
        {
            Debug.Log("Im clicked!", evt.Sender);
        }
    }
}
