using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerReadyStatus : MonoBehaviour
{
	public TextMeshProUGUI TextMeshPro;
	public RectTransform ForegroundLayer;
	
	public void SetReady()
	{
		gameObject.SetActive(true);	
		ForegroundLayer.gameObject.SetActive(false);
		TextMeshPro.outlineWidth = 0.5f;
	}

	public void SetNotReady()
	{
		gameObject.SetActive(true);	
		ForegroundLayer.gameObject.SetActive(true);
		TextMeshPro.outlineWidth = 0f;
	}

	public void Disable()
	{
		gameObject.SetActive(false);
	}
	
	
}
