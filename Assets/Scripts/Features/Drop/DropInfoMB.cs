using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class DropInfoMB : MonoBehaviour
    {
        [SerializeField]
        private DropableItem.ItemType Item;

        private EcsWorldInject World;

        private EcsPool<PickUpNewWeaponEvent> _pickUpNewWeaponEventPool;

        private string _player = "Player";

        public void SetItem(DropableItem.ItemType Item, EcsWorldInject World)
        {
            this.Item = Item;
            this.World = World;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }

            StartPickUpEvent();

            gameObject.SetActive(false);
        }

        private void StartPickUpEvent()
        {
            _pickUpNewWeaponEventPool = World.Value.GetPool<PickUpNewWeaponEvent>();
            ref var pickUpNewWeaponEvent = ref _pickUpNewWeaponEventPool.Add(World.Value.NewEntity());
            pickUpNewWeaponEvent.WeaponType = Item;
        }
    }
}
