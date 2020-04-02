using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopTutorial : MonoBehaviour
{
    // Start is called before the first frame update

    string initialText;
    Vector3 initialPos;
    public float timer = 0f;
    RectTransform myRectTransform;
    Vector2 initialHeight;

    void Start()
    { 
        myRectTransform = GetComponent<RectTransform>(); 
        initialText = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text;

        initialHeight = myRectTransform.sizeDelta;
        initialPos = myRectTransform.localPosition;
    }

    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer>5f)
        {
            GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Press Z to open Tutorial";
            myRectTransform.localPosition = new Vector3(0, -500);
            myRectTransform.sizeDelta = new Vector2(1920, 100);
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>().text = initialText;
            myRectTransform.localPosition = initialPos;
            myRectTransform.sizeDelta = initialHeight;
            timer = 0f;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
