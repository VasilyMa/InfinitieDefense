using UnityEngine;
namespace Client {
    struct InterfaceComponent {
        public GameObject joystick;
        public GameObject resourcePanel;
        public GameObject winPanel;
        public GameObject losePanel;
        public GameObject progressbar;
        public GameObject waveCounter;
        public FloatingJoystick _joystick;
        public Transform _joystickPoint;
        public Transform _joysticKCenter;
    }
}