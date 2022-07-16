using UnityEngine;

namespace Client
{
    struct ContextToolComponent
    {
        public enum Tool { pickaxe, sword, bow, empty };

        public GameObject Pickaxe;
        public GameObject Sword;
        public GameObject Bow;
    }
}