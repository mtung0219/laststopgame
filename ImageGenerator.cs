using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{

    public Texture2D img;

    private HashSet<Color> colorSet = new HashSet<Color>();

    public GameObject colorSquare;

    private Grid grid;
    private int width;
    private int height;
    private Dictionary<Color, int> colordict;
    private Color[,] colorArray;

    public GameObject debugSquaresArray;
    public GameObject debugSquareObject1;

    public GameObject inventoryScreen;

    [SerializeField]
    private Text pictureTitle;

    public string pictureTitle_string;

    [SerializeField]
    private Text percentFilledTitle;

    void Start()
    {
        pictureTitle.text = pictureTitle_string;
        startPalette();
        gridStart();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Color c = inventoryScreen.GetComponent<DisplayColors>().getCurrentColor();
            Vector3 position = GetMouseWorldPosition();
            int x, y;
            grid.GetXYNew(position,out x,out y);
            Color c1 = grid.getXYColor(x, y);
            //Debug.Log(x + " , " + y + " " + position);// + " island number " + grid.GetGridObjectNew(position).getIslandNumber());
            TestGridObject testGridObject = grid.GetGridObjectNew(position);
            if (testGridObject != null && c1 == c)
            {
                testGridObject.unlock();
            }
        }
    }
    private void createPalette()
    {
        foreach (Color p in colorSet)
        {
            var obj = Instantiate(colorSquare);
            obj.GetComponent<SpriteRenderer>().color = p;
        }
    }

    public Dictionary<Color, int> GetColorDict()
    {
        return colordict;
    }

    public Grid getGrid()
    {
        return grid;
    }

    public void setPercentCompleted(int numerator, int denominator)
    {
        percentFilledTitle.text = numerator.ToString() + " / " + denominator.ToString();
    }

    private void startPalette()
    {
        int count = 0;
        width = img.width;
        height = img.height;
        colorArray = new Color[width, height];
        colordict = new Dictionary<Color, int>();
        Debug.Log("image width is " + img.width + " and height is " + img.height);
        //Debug.Log(img.height);
        for (int i = 0; i < img.width; i++)
        {
            for (int j = 0; j < img.height; j++)
            {
                Color pixel = img.GetPixel(i, j);
                colorArray[i, j] = pixel;
                if (!colorSet.Contains(pixel))
                {
                    colorSet.Add(pixel);
                    colordict.Add(pixel, count);
                    count += 1;
                }
            }
        }
        Debug.Log(count + " number of unique colors.");
        //createPalette();
    }
    private void gridStart()
    {
        grid = new Grid(
            width,
            height,
            3f, 
            new Vector3(0,0),
            colorArray,
            colordict,
            debugSquaresArray,
            debugSquareObject1,
            (Grid g, int x, int y, bool b) => new TestGridObject(g,x,y,false));

        inventoryScreen.GetComponent<DisplayColors>().CreateDisplay(colordict, grid);
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}


[System.Serializable]
public class TestGridObject {

    private int value;
    private Grid grid;
    private int x;
    private int y;
    public bool isunlocked;
    private int islandNumber;
    public TestGridObject(Grid grid, int x, int y, bool isunlocked)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isunlocked = isunlocked;
        islandNumber = -1;
    }
    public void unlock()
    {
        //isunlocked = true;
        grid.TriggerGridObjectChanged(x, y);
    }
    public void AddValue(int addValue)
    {
        value += addValue;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool GetIsUnlocked()
    {
        return isunlocked;
    }

    public void setIslandNumber(int i)
    {
        islandNumber = i;
    }

    public int getIslandNumber()
    {
        return islandNumber;
    }
    public override string ToString()
    {
        return value.ToString();
    }
}