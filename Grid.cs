using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
        //public bool unlocked;
    }

    private int width;
    private int height;
    private TestGridObject[,] gridArray; //need to save this
    private TextMesh[,] debugTestArray;
    private float cellSize;
    private Vector3 originPosition;
    private Color[,] colorArray;
    private Dictionary<Color, int> colordict;
    private int numEdges;

    private Graph graph;

    private GameObject[,] debugTestArray1;
    private GameObject debugSquareObject;
    private GameObject debugSquareObject1;

    private GameObject OneBigImage;
    private Sprite s;
    private Texture2D t2d;

    private float spriteRectX;
    private float spriteRectY;
    private int spriteScaleX;
    private int spriteScaleY;
    private float spriteRealWidth;
    private float spriteRealHeight;

    private int filled_in_numerator;
    private int filled_in_denominator;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Color[,] colorArray, Dictionary<Color, int> colordict, GameObject debugSquareArray,
        GameObject debugSquareObject1, Func<Grid,int,int,bool, TestGridObject> createGridObject) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.colorArray = colorArray;
        this.colordict = colordict;
        this.debugSquareObject = debugSquareArray;
        this.debugSquareObject1 = debugSquareObject1;

        gridArray = new TestGridObject[width, height];
        //debugTestArray = new TextMesh[width, height];
        //debugTestArray1 = new GameObject[width, height];

        //INITIALIZE GRAPH AND ADJACENCY LISTS
        graph = new Graph(width * height);
        numEdges = 0;
        filled_in_denominator = width * height;
        filled_in_numerator = 0;
        Set_Percent_Text();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetAdjacentVertices(x, y, colorArray[x, y]);
            }
        }
        Debug.Log(numEdges + " number of edges");

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y, false);
            }
        }
        //*****************************************************************************************************************************
        spriteScaleX = 1000;
        spriteScaleY = 1000;
        OneBigImage = CreateOnePrefabNew("",
                    new Vector3(0,0,0),//GetWorldPosition(0, 0) + new Vector3(cellSize, cellSize) * 0.5f,
                    20,
                    Color.white, spriteScaleX, spriteScaleY, null);
        s = OneBigImage.GetComponentInChildren<SpriteRenderer>().sprite;
        t2d = s.texture;
        bool test = t2d.Resize(width, height);

        //Debug.Log("t2d width is " + t2d.width + " and height is " + t2d.height);
        //Debug.Log("initial pivot is " + s.pivot.x + ", " + s.pivot.y);
        for (int i = 0; i < t2d.width; i++)
        {
            for (int j = 0; j < t2d.height; j++)
            {
                t2d.SetPixel(i, j, Color.gray);
                //t2d.SetPixel(i, j, colorArray[i,j]);
            }
        }
        //OneBigImage.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(t2d, new Rect(0, 0, width, height), new Vector2(0, 0), 100, 0, SpriteMeshType.FullRect);
        t2d.Apply();
        Debug.Log("sprite actual size is " + s.bounds.size.x* spriteScaleX + " , " + s.bounds.size.y* spriteScaleY);
        spriteRealWidth = s.bounds.size.x * spriteScaleX;
        spriteRealHeight = s.bounds.size.y * spriteScaleY;
        spriteRectX = s.rect.x;
        spriteRectY = s.rect.y;

        //Debug.Log("sprite rect is " + s.rect.width + ", " + s.rect.height);
        //g.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(t2d, s.rect, s.pivot, s.pixelsPerUnit);

        //*****************************************************************************************************************************

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                /*debugTestArray1[x, y] = CreateOnePrefab(colordict[colorArray[x, y]].ToString(),
                    GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f,
                    20,
                    colorArray[x, y],1,1);// Color.black);*/

                //this will be its own image later..
                /*debugTestArray[x,y] = CreateWorldText(
                    null,
                    colordict[colorArray[x,y]].ToString(),//gridArray[x, y]?.ToString(),
                    GetWorldPositionNew(x, y),// + new Vector3(cellSize, cellSize) * 0.5f,
                    5,
                    Color.black,
                    TextAnchor.MiddleCenter,
                    TextAlignment.Left, 5000);*/


                //Debug.DrawLine(GetWorldPositionNew(x, y), GetWorldPositionNew(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(GetWorldPositionNew(x, y), GetWorldPositionNew(x+1, y), Color.white, 100f);
            }
        }
        //Debug.DrawLine(GetWorldPositionNew(0, height), GetWorldPositionNew(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPositionNew(width, 0), GetWorldPositionNew(width, height), Color.white, 100f);

        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
        {
            int x_start = eventArgs.x;
            int y_start = eventArgs.y;
            //debugTestArray[x_start, y_start].text = gridArray[x_start, y_start].ToString();
            //debugTestArray[x_start, y_start].color = colorArray[x_start, y_start];

            //colorinSameColorNeighborsUsingGraph2(x_start, y_start, colorArray[x_start, y_start]);
            colorInSameColorNeighborsUsingIslandNumberNew(x_start, y_start, colorArray[x_start, y_start]);
        };



        //SETTING ISLANDS - SHOULD BE HARDCODED LATER
        int islandNo = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (GetGridObject(x, y).getIslandNumber() < 0)
                {
                    FillInIslandNumberUsingGraph2(x, y, islandNo);
                    islandNo += 1;
                }
            }
        }
        Debug.Log("TOTAL NUMBERS OF ISLANDS: " + (islandNo+1));
    }

    private GameObject CreateOnePrefab(string colorNumber, Vector3 pos, int fontSize, Color color, int xScale, int yScale)
    {
        GameObject go = GameObject.Instantiate(debugSquareObject, pos, Quaternion.identity, null);
        go.GetComponentInChildren<TextMeshPro>().text = colorNumber;
        go.GetComponentInChildren<SpriteRenderer>().color = color;
        Transform[] ts = go.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == "Background") t.localScale = new Vector3(xScale, yScale);
        //go.GetComponentInChildren<Transform>().localScale = new Vector3(xScale, yScale, 0);
        return go;
    }
    private GameObject CreateOnePrefabNew(string colorNumber, Vector3 pos, int fontSize, Color color, int xScale, int yScale, Sprite sprite)
    {
        GameObject go = GameObject.Instantiate(debugSquareObject1, pos, Quaternion.identity, null);
        go.GetComponentInChildren<TextMeshPro>().text = colorNumber;
        go.GetComponentInChildren<SpriteRenderer>().color = color;
        if (sprite!=null) go.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        Transform[] ts = go.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == "Background") t.localScale = new Vector3(xScale, yScale);
        //go.GetComponentInChildren<Transform>().localScale = new Vector3(xScale, yScale, 0);
        return go;
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        color.a = 1;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    public void FillInIslandNumberUsingGraph2(int x_start, int y_start, int islandNo)
    {
        Queue<int> q = new Queue<int>();
        int start1d = XYto1D(x_start, y_start);
        int V = width * height;
        List<int> adj = graph.getAdjacentList(start1d);
        bool[] visited = new bool[V];
        for (int i = 0; i < V; i++)
        {
            visited[i] = false;
        }
        q.Enqueue(start1d);
        visited[start1d] = true;
        foreach (int i in adj)
        {
            if (!visited[i]) q.Enqueue(i);
        }
        while (q.Count != 0)
        {
            int d = q.Dequeue();
            int x = Xfrom1D(d);
            int y = Yfrom1D(d);

            GetGridObject(x, y).setIslandNumber(islandNo);
            visited[d] = true;
            adj = graph.getAdjacentList(d);
            foreach (int i in adj)
            {
                if (!visited[i])
                {
                    q.Enqueue(i);
                    visited[i] = true;
                }
            }
        }
    }
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x,y = y });
        }
    }

    public void colorInAllNeighbors(int x_start, int y_start)
    {
        if (x_start >= width || y_start >= height || x_start < 0 || y_start < 0) return;
        //if (debugTestArray[x_start, y_start].text == "done") return;
        //debugTestArray[x_start, y_start].text = "done";
        TestGridObject tgo = GetGridObject(x_start, y_start);
        if (tgo.isunlocked) return;
        tgo.isunlocked = true;
        debugTestArray[x_start, y_start].color = colorArray[x_start, y_start];

        colorInAllNeighbors(x_start + 1, y_start);
        colorInAllNeighbors(x_start - 1, y_start);
        colorInAllNeighbors(x_start, y_start+1);
        colorInAllNeighbors(x_start, y_start-1);
    }

    public void colorinSameColorNeighborsUsingGraph(int x_start, int y_start, Color color)
    {
        int start1D = XYto1D(x_start, y_start);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (colorArray[x, y] == color && !GetGridObject(x, y).isunlocked && graph.isReachable(start1D, XYto1D(x,y)))
                {
                    GetGridObject(x, y).isunlocked = true;
                    debugTestArray[x, y].color = colorArray[x, y];
                }
            }
        }
    }

    public void colorinSameColorNeighborsUsingGraph2(int x_start, int y_start, Color color)
    {
        Queue<int> q = new Queue<int>();
        int start1d = XYto1D(x_start, y_start);
        int V = width * height;
        List<int> adj = graph.getAdjacentList(start1d);
        bool[] visited = new bool[V];
        for (int i = 0; i < V; i++)
        {
            visited[i] = false;
        }
        q.Enqueue(start1d);
        visited[start1d] = true;
        foreach (int i in adj)
        {
            if (!visited[i]) q.Enqueue(i);
        }
        while (q.Count != 0)
        {
            int d = q.Dequeue();
            int x = Xfrom1D(d);
            int y = Yfrom1D(d);

            GetGridObject(x, y).isunlocked = true;
            debugTestArray[x, y].color = colorArray[x, y];
            visited[d] = true;
            adj = graph.getAdjacentList(d);
            foreach (int i in adj)
            {
                if (!visited[i])
                {
                    q.Enqueue(i);
                    visited[i] = true;
                }
            }
        }
    }

    public void colorInSameColorNeighborsUsingIslandNumber(int x_start, int y_start, Color color)
    {
        int islandNumber = GetGridObject(x_start, y_start).getIslandNumber();
        if (GetGridObject(x_start, y_start).isunlocked) return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (GetGridObject(x, y).getIslandNumber() == islandNumber)
                {
                    GetGridObject(x, y).isunlocked = true;
                    //debugTestArray[x, y].color = colorArray[x, y];
                    debugTestArray1[x, y].GetComponentInChildren<SpriteRenderer>().color = colorArray[x, y];
                }
            }
        }
    }

    public void colorInSameColorNeighborsUsingIslandNumberNew(int x_start, int y_start, Color color)
    {
        int islandNumber = GetGridObject(x_start, y_start).getIslandNumber();
        if (GetGridObject(x_start, y_start).isunlocked) return;
        int testcounter = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (GetGridObject(x, y).getIslandNumber() == islandNumber)
                {
                    testcounter += 1;
                    GetGridObject(x, y).isunlocked = true;
                    t2d.SetPixel(x, y, colorArray[x, y]);
                }
            }
        }
        Debug.Log(testcounter + " pixels supposed to be set.");
        t2d.Apply();

        filled_in_numerator += testcounter;

        Set_Percent_Text();

        //MaterialPropertyBlock block = new MaterialPropertyBlock();
        //block.SetTexture("_MainTex", t2d);
        //OneBigImage.GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block);

        //Sprite newSprite = Sprite.Create(t2d, new Rect(0, 0, 4f, 4f), new Vector2(2, 2), 100);
        //s = Resources.Load("testSquareNew", typeof(Sprite)) as Sprite;
        //s = newSprite;
        //OneBigImage.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(t2d, s.rect, s.pivot, s.pixelsPerUnit,0, SpriteMeshType.FullRect);
        //OneBigImage.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(t2d, s.rect, s.pivot, s.pixelsPerUnit,0, SpriteMeshType.FullRect);
        //OneBigImage.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(t2d, new Rect(0,0,4f,4f), new Vector2(0.5f,0.5f),100,0,SpriteMeshType.FullRect);
    }

    private void Set_Percent_Text()
    {
        decimal pct = (decimal) (filled_in_numerator * 100.000M / filled_in_denominator);
        Debug.Log(pct);
        decimal rounded_pct = Math.Round(pct, 2);
        Text fillThisIn = GameObject.FindGameObjectWithTag("percent text").GetComponent<Text>();
        //fillThisIn.text = filled_in_numerator.ToString() + " / " + filled_in_denominator.ToString();
        fillThisIn.text = rounded_pct.ToString() + " %";
    }
    public void colorInSameColorNeighbors(int x_start, int y_start, Color color)
    {
        if (x_start >= width || y_start >= height || x_start < 0 || y_start < 0) return;
        if (colorArray[x_start, y_start] != color) return;

        TestGridObject tgo = GetGridObject(x_start, y_start);
        if (tgo.isunlocked) return;

        tgo.isunlocked = true;
        debugTestArray[x_start, y_start].color = colorArray[x_start, y_start];

        int i = 1;
        while (i < Math.Max(width, height))
        {
            bool neighboringColor = false;
            int x,y;
            for (y = Math.Max(y_start - i,0); y <= Math.Min(y_start + i, height - 1); y++)
            {
                x = Math.Min(x_start + i, width - 1);
                if (colorArray[x, y] == color)
                {
                    debugTestArray[x, y].color = color;
                    neighboringColor = true;
                }
                x = Math.Max(x_start - i, 0);
                if (colorArray[x, y] == color)
                {
                    debugTestArray[x, y].color = color;
                    neighboringColor = true;
                }
            }
            for (x = Math.Max(x_start - i,0); x <= Math.Min(x_start + i,width-1); x++)
            {
                y = Math.Min(y_start + i, height - 1);
                if (colorArray[x, y] == color)
                {
                    debugTestArray[x, y].color = color;
                    neighboringColor = true;
                }
                y = Math.Max(y_start - i, 0);
                if (colorArray[x, y] == color)
                {
                    debugTestArray[x, y].color = color;
                    neighboringColor = true;
                }
            }
            if (!neighboringColor) return;
            i += 1;
        }

        //colorInSameColorNeighbors(x_start + 1, y_start, color);
        //colorInSameColorNeighbors(x_start - 1, y_start, color);
        //colorInSameColorNeighbors(x_start, y_start + 1, color);
        //colorInSameColorNeighbors(x_start, y_start - 1, color);
    }
    private void SetAdjacentVertices(int x, int y, Color color)
    {
        if (x < width-1 && colorArray[x+1,y] == color)
        {
            graph.addEdge(XYto1D(x, y), XYto1D(x + 1, y));
            numEdges += 1;
        }
        if (x > 0 && colorArray[x - 1, y] == color)
        {
            graph.addEdge(XYto1D(x, y), XYto1D(x - 1, y));
            numEdges += 1;
        }
        if (y < height-1 && colorArray[x, y+1] == color)
        {
            graph.addEdge(XYto1D(x, y), XYto1D(x, y+1));
            numEdges += 1;
        }
        if (y > 0 && colorArray[x, y-1] == color)
        {
            graph.addEdge(XYto1D(x, y), XYto1D(x, y-1));
            numEdges += 1;
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private Vector3 GetWorldPositionNew(int x, int y)
    {
        //float xFactor = width * 200f / spriteScaleX;
        //float yFactor = height * 200f / spriteScaleY;

        float xFactor = spriteRealWidth;
        float yFactor = spriteRealHeight;

        float xBorder = xFactor / 2;
        float yBorder = yFactor / 2;
        return new Vector3(-xBorder + x * xFactor / width, -yBorder + y * yFactor / height);
    }

    public Color getXYColor(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return colorArray[x, y];
        }
        return new Color(1,1,1);
    }

    public void setBlackenedColor(Color c)
    {
        //Debug.Log("setting ... " + width + " " + height + " " + t2d);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!GetGridObject(x,y).isunlocked && colorArray[x,y] == c)
                {
                    t2d.SetPixel(x, y, Color.black);
                } else if (!GetGridObject(x, y).isunlocked)
                {
                    t2d.SetPixel(x, y, Color.grey);
                }
            }
        }
        t2d.Apply();
    }

    public void SetGridObject(int x, int y, TestGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTestArray[x, y].text = gridArray[x, y]?.ToString();
        }
    }

    public void SetGridObject(Vector3 worldPosition, TestGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }
    public TestGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        } else
        {
            return default(TestGridObject);
        }
    }
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void GetXYNew(Vector3 worldPosition, out int x, out int y)
    {
        //float xFactor = width * 200f / spriteScaleX;
        //float yFactor = height * 200f / spriteScaleY;

        float xFactor = spriteRealWidth;
        float yFactor = spriteRealHeight;

        float xBorder = xFactor / 2;
        float yBorder = yFactor / 2;

        //Debug.Log("width: " + width + " and height: " + height + "spritescalex " + spriteScaleX);
        x = Mathf.FloorToInt(width * (worldPosition.x + xBorder) / xFactor);
        y = Mathf.FloorToInt(height * (worldPosition.y + yBorder) / yFactor);
    }
    public TestGridObject GetGridObjectNew(Vector3 worldPosition)
    {
        int x, y;
        GetXYNew(worldPosition, out x, out y);
        //Debug.Log("x is " + x + " and y is " + y);
        return GetGridObject(x, y);
    }
    public TestGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public int XYto1D(int x, int y)
    {
        return x + y*width;
    }
    public int Xfrom1D(int v)
    {
        return v % width;
    }

    public int Yfrom1D(int v)
    {
        return v / width;
    }
}


class Graph
    //representing image as an undirected graph. vertices are connected if they are adjacent and of the same color.
{
    int V;
    //int[,] g;
    List<int>[] g1;
    public Graph(int V)
    {
        this.V = V;
        //g = new int[V + 1, V + 1];

        //initialize adjacency matrix
        /*for (int i = 0; i < V + 1; i++)
        {
            for (int j = 0; j < V + 1; j++)
            {
                if (i == j) g[i, j] = 1;
                else g[i, j] = 0;
            }
        }*/

        //initialize adjacency list form
        g1 = new List<int>[V];
        for (int i = 0; i < V; i++)
        {
            g1[i] = new List<int>();
        }
    }

    public void addEdge(int v, int w)
    {
        //g[v, w] = 1;
        //g[w, v] = 1;
        if (!g1[v].Contains(w)) g1[v].Add(w);
        if (!g1[w].Contains(v)) g1[v].Add(v);
        //g1[v].Add(w);
        //g1[w].Add(v);

    }

    public List<int> getAdjacentList(int v)
    {
        return g1[v];
    }

    public bool isReachable(int v, int w)
    {
        if (v == w) return true;
        bool[] visited = new bool[V];
        for (int i = 0; i < V; i++)
        {
            visited[i] = false;
        }
        Queue<int> q = new Queue<int>();

        visited[v] = true;

        q.Enqueue(v);

        while (q.Count != 0)
        {
            v = q.Peek();
            q.Dequeue();

            foreach (int i in g1[v]) //for each i in adjacent vertex list
            {
                if (i == w) return true;

                if (!visited[i])
                {
                    visited[i] = true;
                    q.Enqueue(i);
                }
            }
        }
        return false;
    }
}
