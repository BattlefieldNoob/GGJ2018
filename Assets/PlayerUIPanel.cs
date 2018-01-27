using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIPanel : MonoBehaviour {

	public int id;
	public Image powerUpImage;
	public Image playerImage;

	private void Start()
	{
		SetPowerUpIcon(null); 
	}

	public void PlayerIsAlive()
	{
		playerImage.enabled = true; 
	}

	public void PlayerIsInfected()
	{
		playerImage.color = Color.green; 
	}

	public void PlayerIsNoMoreInfected()
	{
		playerImage.color = Color.white;
	}

	public void PlayerIsDead()
	{
		PlayerIsNoMoreInfected(); 
		playerImage.enabled = false;
	}

	public void SetPowerUpIcon(Sprite sprite)
	{
		print("setting" + sprite); 
		if (sprite == null)
			powerUpImage.enabled = false;
		else
		{
			powerUpImage.enabled = true;
			powerUpImage.sprite = sprite;
		}
	}
}
