using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    public float jumpForce;
    private bool isJumping;
    public Camera mainCamera;
    private int val = 0;
    //public GameObject dialogManager;




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
            if (hit.collider != null && hit.collider.tag == "canpickup")
            {
                var item = hit.collider.gameObject.GetComponent<GroundItem>();

                if (item)
                {
                    inventory.AddItem(new Item(item.item), 1);
                    //inventory.AddItem(item.item, 1);
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            zoomInToLocation();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            zoomOutToLocation();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SHOULD BE SAVING");
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("SHOULD BE LOADING");
            inventory.Load();
        }

        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        Animate();
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<GroundItem>();

        if (item)
        {
            inventory.AddItem(new Item(item.item), 1);
            //inventory.AddItem(item.item, 1);
            Destroy(collision.gameObject);
        }
    }*/

    private void OnApplicationQuit()
    {
        //inventory.Container.Items.Clear();
    }

    private void FixedUpdate()
    {
        //Move();
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }
    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }
        isJumping = false;
    }

    public void zoomInToLocation()
    {
        mainCamera.orthographicSize = 2;
    }
    public void zoomOutToLocation()
    {
        mainCamera.orthographicSize = 5;
    }

}
