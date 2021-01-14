using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayColors : MonoBehaviour
{
    public GameObject colorPrefab;
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;

    public GameObject currentColorDisplay;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private int currentColorInt;
    private Color currentColor;
    private Dictionary<Color, int> colorDict;
    private Grid grid;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private HorizontalLayoutGroup hLayoutGroup;

    // Start is called before the first frame update
    void Start()
    {
        //CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckPointerOverInventoryIcon();
        }
        //UpdateDisplay();
    }

    public void CreateDisplay(Dictionary<Color, int> colorDict, Grid grid)
    {
        this.colorDict = colorDict;
        this.grid = grid;
        int counter = 0;
        foreach (KeyValuePair<Color, int> entry in colorDict)
        {

            GameObject obj = Instantiate(buttonTemplate) as GameObject;
            GameObject lockedIcon = obj.transform.GetChild(1).gameObject;
            obj.SetActive(true);

            Color c = entry.Key;
            Debug.Log("rgb is: " + c.r*255f + " " + c.g * 255f + " " + c.b * 255f);
            string nameCmp = c.r*255f + "_" + c.g * 255f + "_" +  c.b * 255f;
            if (HasItem(nameCmp))
            {
                lockedIcon.SetActive(false);
            } else
            {
                lockedIcon.SetActive(true);
            }
            obj.transform.GetChild(0).GetComponent<Image>().color = c;

            obj.transform.SetParent(buttonTemplate.transform.parent, false);
            counter += 1;
        }
    }

    private bool HasItem(string name)
    { // GOING TO USE ITEM NAME TO COMMUNICATE THAT IT'S A COLOR AND RGB: FORMAT R_G_B (example 255_255_255)
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            if (slot.item.Name == name)
                return true;
        }
        return false;
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot)) {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

            } else
            {
                var obj = Instantiate(colorPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.name = slot.item.Id.ToString();
                //obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItemObjectFromID(slot.item.Id).uiDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container.Items[i].amount.ToString("n0");

                itemsDisplayed.Add(slot, obj);
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + ((-Y_SPACE_BETWEEN_ITEMS) * (i/NUMBER_OF_COLUMN)), 0f);
    }

    public void CheckPointerOverInventoryIcon()
    {
        CheckPointerOverInventoryIcon(GetEventSystemRaycastResults());
    }

    public void CheckPointerOverInventoryIcon(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            //if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            if (curRaysastResult.gameObject.tag == "ColorIcon")
            {
                GameObject lockedIcon = curRaysastResult.gameObject.transform.GetChild(1).gameObject;
                if (!lockedIcon.activeSelf) // only enabled if color is unlocked
                {
                    Color c = curRaysastResult.gameObject.transform.GetChild(0).GetComponent<Image>().color;
                    currentColorInt = colorDict[c];
                    currentColor = c;
                    currentColorDisplay.GetComponent<Image>().color = currentColor;

                    grid.setBlackenedColor(c);
                }
            }

        }
        return;
    }
    public Color getCurrentColor()
    {
        return currentColor;
    }

    public int getCurrentColorInt()
    {
        return currentColorInt;
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
