using UnityEngine;
using System.Collections.Generic;

namespace Client
{
    struct Targetable
    {
        public GameObject TargetObject;
        public int TargetEntity;
        public List<int> AllEntityInDetectedZone;
        public List<int> AllEntityInDamageZone;

        public float DistanceToTarget;
    }
}