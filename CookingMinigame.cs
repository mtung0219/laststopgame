using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigame : MonoBehaviour
{
    [SerializeField]
    private Image Slider;

    private Rigidbody2D rb;

    public int SliderSpeed;
    private bool moveRight = false;
    private bool SliderLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    public void LockInSlider()
    {
        SliderLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SliderLocked) {
            return;
        }
        float prevXPos = Slider.transform.position.x;
        if (moveRight)
        {
            Slider.transform.position = new Vector3(Slider.transform.position.x + SliderSpeed * Time.deltaTime, Slider.transform.position.y, Slider.transform.position.z);
        } else
        {
            Slider.transform.position = new Vector3(Slider.transform.position.x - SliderSpeed * Time.deltaTime, Slider.transform.position.y, Slider.transform.position.z);
        }

        if (Slider.transform.position.x > 8 && prevXPos <= 8)
        {
            moveRight = false;
        } else if (Slider.transform.position.x < -8 && prevXPos >= -8)
        {
            moveRight = true;
        }
    }
}
