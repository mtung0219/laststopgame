using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateArrows : MonoBehaviour
{
    public float startLocation;
    public float locationIncrement;
    public GameObject leftarrowSoul;
    public GameObject rightarrowSoul;
    public GameObject uparrowSoul;
    public GameObject downarrowSoul;
    GameObject arrowclone;
    Queue<GameObject> arrowqueue;
    GameObject[] arrowarray;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public int arraySize;

    private float timeElapsed;
    [SerializeField]
    private Text timerText;

    private int ArrowsCleared;

    public Text ArrowsClearedText;

    // Start is called before the first frame updates
    void Start()
    {
        ArrowsCleared = 0;
        ArrowsClearedText.text = 0.ToString();
        timeElapsed = 20;
        CreateAllArrows();
    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed -= Time.deltaTime;
        timerText.text = timeElapsed.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                string objName = hit.collider.gameObject.name;
                //string firstArrowName = arrowqueue.Peek().gameObject.name;
                string firstArrowName = arrowarray[0].gameObject.name;
                Debug.Log("object name is " + objName);
                Debug.Log("First Arrow name is " + firstArrowName);
                if (objName.Equals("uparrow") && firstArrowName.Equals("testarrowup(Clone)"))
                {
                    ArrowsCleared += 1;
                    ArrowsClearedText.text = ArrowsCleared.ToString();
                    KillFirstArrow(); AddNewArrow();MoveStackDown();
                }
                else if (objName.Equals("downarrow") && firstArrowName.Equals("testarrowdown(Clone)"))
                {
                    ArrowsCleared += 1;
                    ArrowsClearedText.text = ArrowsCleared.ToString();
                    KillFirstArrow(); AddNewArrow(); MoveStackDown();
                }
                else if (objName.Equals("leftarrow") && firstArrowName.Equals("testarrowleft(Clone)"))
                {
                    ArrowsCleared += 1;
                    ArrowsClearedText.text = ArrowsCleared.ToString();
                    KillFirstArrow(); AddNewArrow(); MoveStackDown();
                } else if (objName.Equals("rightarrow") && firstArrowName.Equals("testarrowright(Clone)"))
                {
                    ArrowsCleared += 1;
                    ArrowsClearedText.text = ArrowsCleared.ToString();
                    KillFirstArrow(); AddNewArrow(); MoveStackDown();
                }
            }
        }
    }

    void CreateAllArrows()
    {   
        
        int arrowDir; //between 0 thru 3 for directions

        arrowqueue = new Queue<GameObject>();
        float incrementTotal = 0;
        //while (arrowqueue.Count < 7)
        //{
        //    arrowDir = Random.Range(0, 4);
        //    arrowclone = Instantiate(ArrowDirFromRndNum(arrowDir), new Vector3(transform.position.x, startLocation + incrementTotal, 0), transform.rotation) as GameObject;
        //
        //    arrowqueue.Enqueue(arrowclone);
        //    incrementTotal += locationIncrement;
        //}

        int i = 0;
        arrowarray = new GameObject[arraySize];
        while (i < arraySize)
        {
            arrowDir = Random.Range(0, 4);
            arrowclone = Instantiate(ArrowDirFromRndNum(arrowDir), new Vector3(transform.position.x, startLocation + incrementTotal, 0), transform.rotation) as GameObject;
            arrowarray[i] = arrowclone;
            incrementTotal += locationIncrement;
            i += 1;
        }
    }
    void KillFirstArrow()
    {
        //destroys first arrow on successful click
        //GameObject firstInQ = arrowqueue.Peek();
        //arrowqueue.Dequeue();
        //Destroy(firstInQ, 0);
        Destroy(arrowarray[0]);
        int i = 1;
        while (i < arraySize) //move pointers down one
        {
            arrowarray[i - 1] = arrowarray[i];
            i += 1;
        }
    }

    void AddNewArrow()
    {
        int arrowDir = Random.Range(0, 4);
        float incrementTotal = locationIncrement * 8;
        arrowclone = Instantiate(ArrowDirFromRndNum(arrowDir), new Vector3(transform.position.x, startLocation + incrementTotal, 0), transform.rotation) as GameObject;
        //arrowqueue.Enqueue(arrowclone);

        arrowarray[arraySize-1] = arrowclone;

    }
    void MoveStackDown()
    {
        int i = 0;
        Vector3 moveToPos;
        while (i < arraySize)
        {
            Debug.Log("number " + i + " is " + arrowarray[i].gameObject.name);
            arrowarray[i].transform.position -= new Vector3(0, locationIncrement, 0);
            //moveToPos = arrowarray[i].transform.position - new Vector3(0, locationIncrement, 0);
            //arrowarray[i].transform.position = Vector3.MoveTowards(arrowarray[i].transform.position, moveToPos, Time.deltaTime * 10);
            i += 1;
        }
    }

    public Queue<GameObject> getArrowQueue()
    {
        //getter method for the arrow soul queue
        return arrowqueue;
    }

    GameObject ArrowDirFromRndNum(int num)
    {
        // returns an arrow gameObject depending on which random number was selected
        if (num == 0)
        {
            return leftarrowSoul;
        } else if (num == 1)
        {
            return rightarrowSoul;
        } else if (num == 2)
        {
            return uparrowSoul;
        } else
        {
            return downarrowSoul;
        }
    }
}
