using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ChangeAgentText : MonoBehaviour
{

	public TextMeshProUGUI text;

	// Use this for initialization
	void Start()
	{
		
	}

	

	public void ChangeText(string message)
    {
		text.text = message;
	}

			

}