using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Client {
    sealed class CanvasPointerSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CanvasPointerComponent>> _canvasfilter = default;
        readonly EcsFilterInject<Inc<EnemyTag, UnitTag>, Exc<InactiveTag>> _enemyFilter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        public void Run (EcsSystems systems) {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(enemyEntity);

                foreach (var entityPlayer in _canvasfilter.Value)
                {
                    ref var cameraComp = ref _cameraPool.Value.Get(_state.Value.EntityCamera);
                    ref var pointerComp = ref _canvasfilter.Pools.Inc1.Get(entityPlayer);
                    Vector3 ToEnemy = viewComp.GameObject.transform.position - pointerComp.player.transform.position;
                    Ray ray = new Ray(pointerComp.player.transform.position, ToEnemy);
                    Debug.DrawRay(pointerComp.player.transform.position, ToEnemy);
                    
                    // 0 = left, 1 = right, 2 = down, 3 = up
                    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraComp.CameraTransform.GetComponent<Camera>());

                    float minDistance = Mathf.Infinity;
                    int planeIndex = 0;
                    
                    for (int i = 0; i < 4; i++)
                    {
                        if (planes[i].Raycast(ray, out float distance))
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                planeIndex = i;
                            }
                    }
                    minDistance = Mathf.Clamp(minDistance, 0.0f, ToEnemy.magnitude);
                    Vector3 worldPosition = ray.GetPoint(minDistance);
                    if (ToEnemy.magnitude > minDistance)
                    {
                        viewComp.PointerTransform.localScale = Vector3.one;
                        viewComp.PointerTransform.position = cameraComp.CameraTransform.GetComponent<Camera>().WorldToScreenPoint(worldPosition);
                        viewComp.PointerTransform.rotation = GetIconRotation(planeIndex);
                    }
                    else
                    {
                        viewComp.PointerTransform.localScale = Vector3.one * Time.deltaTime * 1.5f;
                    }
                }
            }
        }
        Quaternion GetIconRotation(int planeIndex)
        {
            if (planeIndex == 0)
                return Quaternion.Euler(0f, 0f, 90f);
            else if (planeIndex == 1)
                return Quaternion.Euler(0f, 0f, -90f);
            else if (planeIndex == 2)
                return Quaternion.Euler(0f, 0f, 180f);
            else if (planeIndex == 3)
                return Quaternion.Euler(0f, 0f, 0f);
            return Quaternion.identity;
        }
    }
}