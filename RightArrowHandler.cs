using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArrowHandler : MonoBehaviour
{
    public GameObject arrowGenerator;
    Queue<GameObject> aQueue;

    // Start is called before the first frame update
    void Start()
    {
        CreateArrows AG = arrowGenerator.GetComponent<CreateArrows>();
        aQueue = AG.getArrowQueue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
