using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideWalls : MonoBehaviour
{
    public GameObject mesh;
    public Mesh[] wallModels;

    public void toggleWalls(bool hidden) 
    {
        if (hidden) 
        {
            mesh.GetComponent<MeshFilter>().mesh = wallModels[1];
            Debug.Log("Walls = Wall Model 1");
        }
        else 
        {
            mesh.GetComponent<MeshFilter>().mesh = wallModels[0];
            Debug.Log("Walls = Wall Model 2");
        }
    }
}
