using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIPanel : MonoBehaviour {

	public int id;
	public Image powerUpImage;
	public Image playerImage;
    public Image alienImage; 

	public void Present()
	{
		transform.GetChild(0).gameObject.SetActive(true);
        playerImage.enabled = true;
        alienImage.enabled = false; 
        SetPowerUpIcon(null);
	}

	public void PlayerIsAlive()
	{
		playerImage.enabled = true; 
	}

	public void PlayerIsInfected()
	{
        alienImage.enabled = true;  
	}

	public void PlayerIsNoMoreInfected()
	{
        alienImage.enabled = false;
    }

	public void PlayerIsDead()
	{
		PlayerIsNoMoreInfected(); 
		playerImage.enabled = false;
		SetPowerUpIcon(null);
	}

	public void SetPowerUpIcon(Sprite sprite)
	{
		//print("setting" + sprite); 
		if (sprite == null)
			powerUpImage.enabled = false;
		else
		{
			powerUpImage.enabled = true;
			powerUpImage.sprite = sprite;
		}
	}
}
