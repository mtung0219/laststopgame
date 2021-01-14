using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationPuzzle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] GameObjectPieces;

    [SerializeField]
    private GameObject[] GameObjectPiecesSolution;

    private Vector3[] OriginalPieceVectors;
    bool isPicked;
    private int clickedPiece;
    private float yDisplacement;
    private float xDisplacement;

    public float AllowedDeviation;

    // Start is called before the first frame update
    void Start()
    {
        OriginalPieceVectors = new Vector3[GameObjectPieces.Length];
        for (int i = 0; i < GameObjectPieces.Length; i++)
        {
            OriginalPieceVectors[i] = GameObjectPieces[i].transform.position;
        }
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
                for (int i = 0; i < GameObjectPieces.Length; i++)
                {
                    if (hit.collider.gameObject.name == GameObjectPieces[i].name)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        clickedPiece = i;

                        yDisplacement = mousePos2D.y - GameObjectPieces[i].transform.position.y;
                        xDisplacement = mousePos2D.x - GameObjectPieces[i].transform.position.x;
                    }
                }
                isPicked = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isCorrect())
            {
                GameObjectPieces[clickedPiece].transform.position = GameObjectPiecesSolution[clickedPiece].transform.position;
                //GameObjectPieces[clickedPiece].GetComponent<BoxCollider2D>().
            } else
            {
                GameObjectPieces[clickedPiece].transform.position = OriginalPieceVectors[clickedPiece];
            }
            isPicked = false;

        }
        if (isPicked == true)
        {
            GameObjectPieces[clickedPiece].transform.position = new Vector2(mousePos.x - xDisplacement, mousePos.y - yDisplacement);
        }

    }

    bool isCorrect()
    {
        if (Mathf.Abs(GameObjectPieces[clickedPiece].transform.position.x - GameObjectPiecesSolution[clickedPiece].transform.position.x) < AllowedDeviation &&
            Mathf.Abs(GameObjectPieces[clickedPiece].transform.position.y - GameObjectPiecesSolution[clickedPiece].transform.position.y) < AllowedDeviation)
        {
            return true;
        }
        return false;
    }
}
