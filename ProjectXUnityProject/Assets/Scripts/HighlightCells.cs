using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightCells : MonoBehaviour
{
    private List<GameObject> highlights;
    private GameObject highlight;
    private Vector3 mousePos;
    private GameObject mousePosHighlight;

    private static HighlightCells _instance;
    public static HighlightCells instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;

        highlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
        highlight.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        highlight.transform.Rotate(Vector3.right, 90);

        highlights = new List<GameObject>();

        mousePosHighlight = GameObject.CreatePrimitive(PrimitiveType.Quad);
        mousePosHighlight.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        mousePosHighlight.transform.Rotate(Vector3.right, 90);
    }

    public void PlaceHighlight(Vector2Int position)
    {
        bool found = false;
        Vector3 pos = new Vector3(position.x, 0.01f, position.y);
        foreach (GameObject go in highlights) if (go.transform.position == pos) found = true;
        if (!found) highlights.Add(Instantiate(highlight, pos, highlight.transform.rotation));
    }
    public void PlaceHighlight(Vector2Int position,Color color)
    {
        bool found = false;
        Vector3 pos = new Vector3(position.x, 0.01f, position.y);
        foreach (GameObject go in highlights) if (go.transform.position == pos) found = true;
        if (!found)
        {
            highlights.Add(Instantiate(highlight, pos, highlight.transform.rotation));
            highlights[highlights.Count - 1].GetComponent<Renderer>().material.color = color;
        }
    }

    public void ClearHighlights()
    {
        if (highlights!=null&&highlights.Count > 0)
        {
            foreach (GameObject go in highlights) Destroy(go.gameObject);
            highlights.Clear();
        }
    }

    public void FixedUpdate()
    {
        //set up ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //create a plane at floor level
        Plane hitPlane = new Plane(Vector3.up, new Vector3(0, -0.5f, 0));
        //Plane.Raycast stores the distance from ray.origin to the hit point in this variable
        float distance = 0;
        //if the ray hits the plane
        if (hitPlane.Raycast(ray, out distance))
        {
            //get the hit point
            mousePos = ray.GetPoint(distance);
            mousePosHighlight.transform.position = new Vector3(Mathf.FloorToInt(mousePos.x),
                0 , Mathf.FloorToInt(mousePos.z) )+ 
                new Vector3(0,0.01f,0);
        }
    }
}
