using UnityEngine;
namespace Client {
    struct DamagePopupEvent {
        public int DamageAmount;
        public GameObject DamageObject;
        public Vector3 target;
    }
}