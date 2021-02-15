using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Text text;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("mouse pos:{0}\nmouse world pos:{1}",Input.mousePosition.ToString(),cam.ScreenToWorldPoint(Input.mousePosition));
    }
}
