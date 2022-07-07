using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Client {
    sealed class CanvasShipPointerSystem : IEcsRunSystem {
        readonly EcsSharedInject<GameState> _state = default;
        readonly EcsFilterInject<Inc<CanvasPointerComponent>> _canvasfilter = default;
        readonly EcsFilterInject<Inc<ShipTag, ShipComponent>, Exc<InactiveTag>> _enemyActiveFilter = default;
        readonly EcsFilterInject<Inc<ShipTag, ShipComponent, InactiveTag>> _enemyInactiveFilter = default;
        readonly EcsPoolInject<ViewComponent> _viewPool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        public void Run (EcsSystems systems) {
            foreach (var enemyEntity in _enemyActiveFilter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(enemyEntity);

                foreach (var entityPlayer in _canvasfilter.Value)
                {
                    ref var cameraComp = ref _cameraPool.Value.Get(_state.Value.EntityCamera);
                    ref var pointerComp = ref _canvasfilter.Pools.Inc1.Get(entityPlayer);
                    viewComp.PointerTransform.gameObject.SetActive(true);
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
            foreach (var enemyEntity in _enemyInactiveFilter.Value)
            {
                ref var viewComp = ref _viewPool.Value.Get(enemyEntity);
                viewComp.PointerTransform.gameObject.SetActive(false);
            }
        }
        Quaternion GetIconRotation(int planeIndex)
        {
            switch (planeIndex)
            {
                case 0:     return Quaternion.Euler(0f, 0f, 90f);
                case 1:     return Quaternion.Euler(0f, 0f, -90f);
                case 2:     return Quaternion.Euler(0f, 0f, 180f);
                case 3:     return Quaternion.Euler(0f, 0f, 0f);
                default:    return Quaternion.identity;
            }
        }
    }
}