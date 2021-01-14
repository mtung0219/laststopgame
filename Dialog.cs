using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public string[] coldNPC_initial;
    public string[] coldNPC_fulfilled;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public Animator textDisplayAnim;
    private bool dialogActive;
    public GameObject dialogBackground;
    private string[] sentenceToUse = null;
    public InventoryObject inventory;

    [SerializeField]
    private ItemObject KeyTwo;

    private int val = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogActive = false;
        //LoadGeneric();
    }

    public void LoadColdNPC_initial()
    {
        if (HasItem(6))
        {
            sentenceToUse = coldNPC_fulfilled;
        } else
        {
            sentenceToUse = coldNPC_initial;
        }
        testDialogStart();
    }

    public void LoadGeneric()
    {
        sentenceToUse = sentences;
        testDialogStart();
    }

    public void testDialogStart()
    {
        dialogBackground.SetActive(true);
        StartCoroutine(Dialog1());
    }

    IEnumerator Dialog1()
    {
        textDisplay.maxVisibleCharacters = 0;
        textDisplay.text = sentenceToUse[index];
        //yield return new WaitForSeconds(typingSpeed);
        foreach (char letter in sentenceToUse[index].ToCharArray())
        {
            textDisplay.maxVisibleCharacters += 1;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);
        if (index < sentenceToUse.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Dialog1());
        } else
        { // end of dialog spiel
            textDisplay.text = "";
            continueButton.SetActive(false);
            dialogBackground.SetActive(false);
            index = 0;
            dialogActive = false;

            //AddKeyTest();
        }
    }

    private bool HasItem(int id)
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            if (slot.item.Id == id)
                return true;
        }
        return false;
    }

    public void DisplayItemDescription(string desc)
    {
        dialogBackground.SetActive(true);
        textDisplay.text = desc;
    }

    private void AddKeyTest()
    {
        inventory.AddItem(new Item(KeyTwo), 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (sentenceToUse != null)
        {
            if (textDisplay.text == sentenceToUse[index])
                {
                    continueButton.SetActive(true);
                }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);



            if (hit.collider != null)
            {
                string objName = hit.collider.gameObject.name;
                Debug.Log("object name is " + objName);
                if (objName.Equals("ColdNPC") && !dialogActive)
                {
                    dialogActive = true;
                    LoadColdNPC_initial();
                }
                if (objName.Equals("rightarrow") && !dialogActive)
                {
                    dialogActive = true;
                    testDialogStart();
                }
                return;
            }
            CheckPointerOverInventoryIcon();
        }
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
            if (curRaysastResult.gameObject.tag == "InventoryIcon")
            {
                System.Int32.TryParse(curRaysastResult.gameObject.name, out val);
                int itemID = val;

                ItemObject io = inventory.database.GetItemObjectFromID(itemID);

                DisplayItemDescription(io.description);
            }
                
        }
        return;
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
    public bool getDialogActive()
    {
        return dialogActive;
    }
}
