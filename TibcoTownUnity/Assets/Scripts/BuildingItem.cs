using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "City Build/BuildingItem")]
public class BuildingItem : ScriptableObject {

    public Sprite sprite;
    public int price;
    public int contructionTime;
    public int profitValue;
    public int profitTime;
    public GameObject buildItemPrefab;

}

[CreateAssetMenu (menuName = "City Build/Outlets")]
public class OutletList : ScriptableObject
{
    public List<BuildingItem> outlets;
}