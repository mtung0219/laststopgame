using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjTextControl : MonoBehaviour
{
    Text thisText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(MenuHandlerScript.Objective obj)
    {
        thisText = GetComponent<Text>();
        thisText.text = obj.objDesc;
    }
}
