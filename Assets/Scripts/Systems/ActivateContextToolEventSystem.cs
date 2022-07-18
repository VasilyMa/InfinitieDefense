using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ActivateContextToolEventSystem : IEcsRunSystem
    {

        readonly EcsFilterInject<Inc<Player, ContextToolComponent, ActivateContextToolEvent>> _ActivateContextToolFilter = default;
        readonly EcsPoolInject<ContextToolComponent> _contextToolPool = default;
        readonly EcsPoolInject<ActivateContextToolEvent> _activateContextToolPool = default;

        public void Run (EcsSystems systems)
        {
            foreach (var playerEntity in _ActivateContextToolFilter.Value)
            {
                ref var contextToolComponent = ref _contextToolPool.Value.Get(playerEntity);
                ref var activateContextToolComponent = ref _activateContextToolPool.Value.Get(playerEntity);

                if (contextToolComponent.CurrentActiveTool != ContextToolComponent.Tool.empty)
                {
                    contextToolComponent.ToolsPool[((int)contextToolComponent.CurrentActiveTool)].SetActive(false);
                }

                if (activateContextToolComponent.ActiveTool != ContextToolComponent.Tool.empty)
                {
                    contextToolComponent.ToolsPool[((int)activateContextToolComponent.ActiveTool)].SetActive(true);
                }

                contextToolComponent.CurrentActiveTool = activateContextToolComponent.ActiveTool;

                _activateContextToolPool.Value.Del(playerEntity);
            }
        }
    }
}