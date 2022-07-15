using UnityEngine;

namespace Client
{
    struct ContextToolComponent
    {
        public enum Tool { pickaxe, sword, bow, empty };

        public Tool ActiveTool;

        public GameObject Pickaxe;
        public GameObject Sword;
        public GameObject Bow;
    }
}