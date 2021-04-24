using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolTipFollow : MonoBehaviour
{ 
    void FixedUpdate()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        

        if (Input.mousePosition.x > Screen.width / 2) 
        {
            localPoint.x -= GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().itemToolTipText.preferredWidth;
        }

        transform.localPosition = localPoint;
    }
}
