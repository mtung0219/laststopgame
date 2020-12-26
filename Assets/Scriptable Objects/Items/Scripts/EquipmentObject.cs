using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float atkBonus;
    public float defenseBonus;
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
