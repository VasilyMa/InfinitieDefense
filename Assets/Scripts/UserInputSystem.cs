using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;
namespace Client {
    sealed class UserInputSystem : EcsUguiCallbackSystem
    {
        //readonly EcsFilterInject<Inc<EcsUguiClickEvent>> _filter = default;
        
        [Preserve]
        [EcsUguiClickEvent(Idents.Ui.Forward, Idents.Worlds.Events)]
        void OnClickForward(in EcsUguiClickEvent evt)
        {
            Debug.Log("Click!", evt.Sender);
        }
        [Preserve]
        [EcsUguiDragStartEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDownTouchListner(in EcsUguiDragStartEvent evt)
        {
            Debug.Log("Down", evt.Sender);
        }
        [Preserve]
        [EcsUguiDragMoveEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnDragTouchListner(in EcsUguiDragMoveEvent evt)
        {
            Debug.Log("Move", evt.Sender);
        }
        [Preserve]
        [EcsUguiDragEndEvent(Idents.Ui.TouchListener, Idents.Worlds.Events)]
        void OnUpTouchListner(in EcsUguiDragEndEvent evt)
        {
            Debug.Log("Up", evt.Sender);
        }
    }
}
