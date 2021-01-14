using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesControl : MonoBehaviour
{
    private List<MenuHandlerScript.Objective> genericObjectives;

    [SerializeField]
    private GameObject objTextTemplate;

    [SerializeField]
    private VerticalLayoutGroup vertGroup;

    [SerializeField]
    //private Sprite[] iconSprites;
    private string[] iconStrings;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenObjectives(List<MenuHandlerScript.Objective> genericObjectives)
    {
        /*if (genericObjectives.Count < 11)
        {
            gridGroup.constraintCount = genericObjectives.Count;
        }
        else
        {
            gridGroup.constraintCount = 10;
        }*/

        foreach (MenuHandlerScript.Objective newObj in genericObjectives)
        {
            GameObject newText = Instantiate(objTextTemplate);
            newText.SetActive(true);
            newText.GetComponent<ObjTextControl>().setText(newObj);
            newText.transform.SetParent(objTextTemplate.transform.parent, false);
        }
    }
}
