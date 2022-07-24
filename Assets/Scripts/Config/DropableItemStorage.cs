using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropableItemStorage", menuName = "Configs/DropableItemStorage", order = 0)]
public class DropableItemStorage : ScriptableObject
{
    // Info from DropableItem.ItemType
    [Header("0 - Sword, 1 - Bow")]
    public GameObject[] DroppedWeaponPrefab;
}
