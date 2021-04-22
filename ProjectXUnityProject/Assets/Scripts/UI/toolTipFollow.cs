using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolTipFollow : MonoBehaviour
{ 
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        if (Input.mousePosition.x < Screen.width / 2)
        {
            transform.localPosition = localPoint;
        }
        else 
        {
            localPoint.x = GameObject.FindGameObjectWithTag("UI").GetComponent<UIHandling>().getTooltipSize();
            transform.localPosition = localPoint;
        }
    }
}
