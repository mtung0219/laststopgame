using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayDatabase : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public ItemDatabaseObject db;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;

    //Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
    }
    // Update is called once per frame
    void Update()
    {
        //UpdateDisplay();
    }
    /*public void CreateDisplay()
    {
        int i = 0;
        foreach(KeyValuePair<int, ItemObject> entry in db.GetItem)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = entry.Value.uiDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            //obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

            //itemsDisplayed.Add(slot, obj);
            i++;
        }
    }*/

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + ((-Y_SPACE_BETWEEN_ITEMS) * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
