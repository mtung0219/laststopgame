using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandlerScript : MonoBehaviour
{
    public GameObject WeaponInventory;
    public GameObject FurnitureInventory;
    public GameObject NewRoomInventory;

    public GameObject Objectives;

    public GameObject buildMenuCanvas;

    private List<PlayerItem> weaponInventory;
    private List<PlayerItem> furnitureInventory;
    private List<PlayerItem> newRoomInventory;
    private List<Objective> thisLevelObjective;

    // Start is called before the first frame update
    void Start()
    {
        //SETTING THE VARIOUS INVENTORY LISTS FROM HERE. PROBABLY CHANGE LATER
        weaponInventory = new List<PlayerItem>();
        for (int i = 1; i <= 100; i++)
        {
            PlayerItem newItem = new PlayerItem();
            newItem.buttonString = i.ToString();
            newItem.setAttack(20);
            newItem.setCost(20);
            newItem.setDescription("weapon desc");
            newItem.setHP(100);
            weaponInventory.Add(newItem);
        }
        furnitureInventory = new List<PlayerItem>();
        for (int i = 1; i <= 100; i++)
        {
            PlayerItem newItem = new PlayerItem();
            newItem.buttonString = i.ToString();
            newItem.setAttack(20);
            newItem.setCost(20);
            newItem.setDescription("furniture desc");
            newItem.setHP(100);
            furnitureInventory.Add(newItem);
        }
        newRoomInventory = new List<PlayerItem>();
        for (int i = 1; i <= 100; i++)
        {
            PlayerItem newItem = new PlayerItem();
            newItem.buttonString = i.ToString();
            newItem.setAttack(20);
            newItem.setCost(20);
            newItem.setDescription("new room desc");
            newItem.setHP(100);
            newRoomInventory.Add(newItem);
        }

        thisLevelObjective = new List<Objective>();
        for (int i = 1; i <= 4; i++)
        {
            Objective newObjective = new Objective();
            newObjective.setDesc("Keep Giovanna alive.");
            newObjective.setIsBonus(false);
            newObjective.setObjCompleted(false);

            thisLevelObjective.Add(newObjective);
        }

        WeaponInventory.GetComponentInChildren<InventoryControl>().GenInventory(weaponInventory);
        FurnitureInventory.GetComponentInChildren<InventoryControl>().GenInventory(furnitureInventory);
        NewRoomInventory.GetComponentInChildren<InventoryControl>().GenInventory(newRoomInventory);
        Objectives.GetComponentInChildren<ObjectivesControl>().GenObjectives(thisLevelObjective);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void openWeaponInventoryCanvas()
    {
        WeaponInventory.SetActive(true);
        buildMenuCanvas.SetActive(false);
    }
    public void closeWeaponInventoryCanvas()
    {
        WeaponInventory.SetActive(false);
    }
    public void openFurnitureInventoryCanvas()
    {
        FurnitureInventory.SetActive(true);
        buildMenuCanvas.SetActive(false);
    }
    public void closeFurnitureInventoryCanvas()
    {
        FurnitureInventory.SetActive(false);
    }
    public void openNewRoomInventoryCanvas()
    {
        NewRoomInventory.SetActive(true);
        buildMenuCanvas.SetActive(false);
    }
    public void closeNewRoomInventoryCanvas()
    {
        NewRoomInventory.SetActive(false);
    }
    public void openBuildMenuCanvas()
    {
        buildMenuCanvas.SetActive(true);
    }

    public void closeBuildMenuCanvas()
    {
        buildMenuCanvas.SetActive(false);
    }
    public void openObjectiveMenuCanvas()
    {
        Objectives.SetActive(true);
    }

    public void closeObjectiveMenuCanvas()
    {
        Objectives.SetActive(false);
    }

    public class Objective
    {
        public string objDesc; //description of objective
        public bool objCompleted; //is objective done or not
        public bool isBonus; //is objective a bonus

        public void setDesc(string objDesc)
        {
            this.objDesc = objDesc;
        }
        public void setObjCompleted(bool objCompleted)
        {
            this.objCompleted = objCompleted;
        }
        public void setIsBonus(bool isBonus)
        {
            this.isBonus = isBonus;
        }


    }
    public class PlayerItem
    {
        public Sprite iconSprite;
        public string buttonString;
        public string description; //description of the item
        public int cost; //how much "data' this item costs
        public int attack; //how much attack points it adds to sanctuary, if applicable
        public int hp; //how much hp or defense it adds to sanctuary, if applicable
        public bool isLocked; //is the current item locked?

        public void setDescription(string description)
        {
            this.description = description;
        }
        public void setCost(int cost)
        {
            this.cost = cost;
        }
        public void setAttack(int attack)
        {
            this.attack = attack;
        }
        public void setHP(int hp)
        {
            this.hp = hp;
        }
        public void setLocked(bool isLocked)
        {
            this.isLocked = isLocked;
        }
    }
}
