using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private ItemObject[] Items;

    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();

    public void OnAfterDeserialize()
    {
        Debug.Log("AFTER SERIALIZE " + Items.Length);
        Debug.Log("Screen width is " + Screen.width + " and height is " + Screen.height);

        for (int i = 0; i < Items.Length; i++)
        {
            //Items[i].Id = i;
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].Id = i;
        }
        GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        //Debug.Log("BEFORE SERIALIZE " + Items[1].description + " " + Items[2].description);
    }
}
