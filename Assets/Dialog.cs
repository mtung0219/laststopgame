using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public Animator textDisplayAnim;
    private bool dialogActive;
    public GameObject dialogBackground;

    // Start is called before the first frame update
    void Start()
    {
        dialogActive = false;
    }

    public void testDialogStart()
    {
        dialogBackground.SetActive(true);
        StartCoroutine(Dialog1());
    }

    IEnumerator Dialog1()
    {
        textDisplay.maxVisibleCharacters = 0;
        textDisplay.text = sentences[index];
        //yield return new WaitForSeconds(typingSpeed);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.maxVisibleCharacters += 1;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Dialog1());
        } else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            dialogBackground.SetActive(false);
            index = 0;
            dialogActive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                string objName = hit.collider.gameObject.name;
                Debug.Log("object name is " + objName);
                if (objName.Equals("rightarrow") && !dialogActive)
                {
                    dialogActive = true;
                    testDialogStart();
                }
            }
        }
    }

    public bool getDialogActive()
    {
        return dialogActive;
    }
}
