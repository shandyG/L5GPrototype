using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpatialMap : MonoBehaviour
{

    public GameObject PinkCube;
    public GameObject DisplayWorld;

    //public Material White;
    //public Material Trans;
    // Start is called before the first frame update
    
    public void ChangeWorldColor()
    {
        DisplayWorld.layer = 15; //layerを15 minimapに変更する
        Debug.Log("pi!");
    }

    
}
