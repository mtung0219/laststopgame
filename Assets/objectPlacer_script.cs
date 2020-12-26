using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPlacer_script : MonoBehaviour
{
    bool isPicked;
    public GameObject testObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Vector2 mousePos2DRounded = new Vector2(Mathf.Round(mousePos2D.x), Mathf.Round(mousePos2D.y) * 0.5f);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                isPicked = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("ISPICKED IS NOW FALSE");
            isPicked = false;
        }
        if (isPicked == true)
        {
            
            testObject.transform.position = mousePos2DRounded;
        }
    }
}
