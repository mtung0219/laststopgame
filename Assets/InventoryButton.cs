using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    //[SerializeField]
    private Image myIcon;

    [SerializeField]
    private Text myText;

    [SerializeField]
    private Text desc;
    [SerializeField]
    private Text atk;
    [SerializeField]
    private Text cst;
    [SerializeField]
    private Text hp;

    private MenuHandlerScript.PlayerItem PI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIcon(Sprite mySprite)
    {
        myIcon.sprite = mySprite;
    }

    public void SetString(string myString)
    {
        myText.text = myString;
    }
    public void SetPI(MenuHandlerScript.PlayerItem playerItem)
    {
        PI = playerItem;
    }

    public void setCaptions()
    {
        desc.text = PI.description;
        atk.text = "Attack: " + PI.attack.ToString();
        cst.text = "Cost: " + PI.cost.ToString();
        hp.text = "HP: " + PI.hp.ToString();

    }
}
