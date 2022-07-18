using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower
{
    public int TowerLevel;
    public int TowerHealth;
    public GameObject TowerPrefab;
    public Mesh TowerMesh;
    public Sprite ImageResource;
    public int Radius;
    public int Upgrade;
    public bool IsLast;
    public string NextID;
    public int DefenderCount;
}
