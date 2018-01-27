using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerReadyStatus : MonoBehaviour
{
	public TextMeshProUGUI TextMeshPro;
	public RectTransform ForegroundLayer;
	public bool isReady = false;
	public bool isEnabled = false;
	
	public void SetReady()
	{
		gameObject.SetActive(true);	
		ForegroundLayer.gameObject.SetActive(false);
		TextMeshPro.outlineWidth = 0.5f;
		isReady = true;
		isEnabled = true;
	}

	public void SetNotReady()
	{
		gameObject.SetActive(true);	
		ForegroundLayer.gameObject.SetActive(true);
		TextMeshPro.outlineWidth = 0f;
		isReady = false;
		isEnabled = true;
	}

	public void Disable()
	{
		isReady = false;
		isEnabled = false;
		gameObject.SetActive(false);
	}
	
	
}
