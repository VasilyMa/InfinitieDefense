using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InterfaceStorage", menuName = "Configs/InterfaceStorage", order = 0)]
public class InterfaceStorage : ScriptableObject
{
    public GameObject RadiusPrefab;
    public GameObject UpgradePointPrefab;
    public GameObject DefenceTowerUpgradePointPrefab;
    public GameObject GoldPrefab;
    public GameObject RockPrefab;
    public GameObject Point;
    public GameObject Slider;
}
