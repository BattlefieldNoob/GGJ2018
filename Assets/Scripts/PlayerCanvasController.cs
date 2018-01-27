using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour {

	

	public void InitUI(List<Color> colors)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			child.GetComponentInChildren<Text>().text = i.ToString();
			child.Find("Color").GetComponent<Image>().color = colors[i];
		}
	}
}
