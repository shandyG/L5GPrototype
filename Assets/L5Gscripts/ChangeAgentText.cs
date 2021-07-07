using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAgentText : MonoBehaviour
{

	public Text text;

	// Use this for initialization
	void Start()
	{
		
	}

	

	public void ChangeText(string message)
    {
		text.text = message;
		Debug.Log(message);
	}

			

}