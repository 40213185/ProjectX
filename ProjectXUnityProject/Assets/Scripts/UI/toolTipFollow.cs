using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolTipFollow : MonoBehaviour
{ 
    void Update()
    {
            gameObject.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
    }
}
