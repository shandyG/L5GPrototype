using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpatialMap : MonoBehaviour
{

    public GameObject PinkCube;
    public GameObject DisplayWorld;

    public Material White;
    public Material Trans;
    // Start is called before the first frame update
    
    public void ChangeWorldColor()
    {
        DisplayWorld.GetComponent<Renderer>().material = Trans;
        Debug.Log("pi!");
    }

    
}
