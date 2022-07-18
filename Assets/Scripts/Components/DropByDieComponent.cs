using System.Collections.Generic;
using UnityEngine;

namespace Client {
    struct DropByDieComponent
    {
        public List <Transform> Transform;
        public Vector3 StartPosition;
        public Vector3 TargetPosition;
        public bool Coin;
        public float Speed;
        public GameObject Object; 
    }
}