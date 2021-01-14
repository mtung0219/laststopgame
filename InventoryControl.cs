using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryControl : MonoBehaviour
{
    private List<MenuHandlerScript.PlayerItem> genericInventory;

    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    [SerializeField]
    //private Sprite[] iconSprites;
    private string[] iconStrings;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GenInventory(List<MenuHandlerScript.PlayerItem> genericInventory)
    {
        if (genericInventory.Count <4)
        {
            gridGroup.constraintCount = genericInventory.Count;
        } else
        {
            gridGroup.constraintCount = 4;
        }

        foreach (MenuHandlerScript.PlayerItem newItem in genericInventory)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            //newButton.GetComponent<InventoryButton>().SetIcon(newItem.iconSprite);
            newButton.GetComponent<InventoryButton>().SetString(newItem.buttonString);
            newButton.GetComponent<InventoryButton>().SetPI(newItem);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
