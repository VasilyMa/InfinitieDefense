using UnityEngine;
using UnityEngine.UI;

namespace Client {
    struct LevelUpEvent {
        public GameObject LevelPopUp;
        public Vector3 target;
        public Vector3 Pos;
        public Text Text;
        public float TimeOut;
    }
}