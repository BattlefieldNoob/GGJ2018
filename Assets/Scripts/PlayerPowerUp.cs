using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour {

	PlayerStatus Status;
	CharacterMovement Movement;

	PowerUp.PowerUps CurrentPowerUp;
	private bool HasPowerUp;
	// Use this for initialization
	void Start () {
		Status = GetComponent<PlayerStatus>();
		Movement = GetComponent<CharacterMovement>();
		HasPowerUp = false;
	}

	public bool HasPower()
	{
		return HasPowerUp;
	}
	
	public void SetPowerUp(PowerUp.PowerUps p)
	{
		CurrentPowerUp = p;
		HasPowerUp = true;
	}

	// Update is called once per frame
	void Update () {
		if(HasPowerUp && Input.GetButtonDown("Button" + Movement.PlayerID) && !Status.IsInfected())
		{
			print("CACCA");
		}
	}
}
