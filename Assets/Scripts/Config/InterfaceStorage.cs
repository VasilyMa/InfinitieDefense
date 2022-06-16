using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InterfaceStorage", menuName = "Configs/InterfaceStorage", order = 0)]
public class InterfaceStorage : ScriptableObject
{
    public GameObject RadiusPrefab;
    public GameObject UpgradePointPrefab;
    public GameObject GoldPrefab;
}
