using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ActivateContextToolEventSystem : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ContextToolComponent, ActivateContextToolEvent, ViewComponent>> _ActivateContextToolFilter = default;
        readonly EcsPoolInject<ContextToolComponent> _contextToolPool = default;
        readonly EcsPoolInject<ActivateContextToolEvent> _activateContextToolPool = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var playerEntity in _ActivateContextToolFilter.Value)
            {
                ref var contextToolComponent = ref _contextToolPool.Value.Get(playerEntity);
                ref var activateContextToolComponent = ref _activateContextToolPool.Value.Get(playerEntity);
                ref var viewComponent = ref _viewPool.Value.Get(playerEntity);

                if (contextToolComponent.CurrentActiveTool != ContextToolComponent.Tool.empty)
                {
                    contextToolComponent.ToolsPool[((int)contextToolComponent.CurrentActiveTool)].SetActive(false);
                }

                if (activateContextToolComponent.ActiveTool != ContextToolComponent.Tool.empty)
                {
                    contextToolComponent.ToolsPool[((int)activateContextToolComponent.ActiveTool)].SetActive(true);

                    viewComponent.Animator.SetLayerWeight(1, 1);
                }
                else
                {
                    viewComponent.Animator.SetLayerWeight(1, 0);
                }

                contextToolComponent.CurrentActiveTool = activateContextToolComponent.ActiveTool;

                _activateContextToolPool.Value.Del(playerEntity);
            }
        }
    }
}