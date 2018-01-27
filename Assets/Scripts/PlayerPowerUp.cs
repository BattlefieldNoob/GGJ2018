using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour {

	PlayerStatus Status;
	CharacterMovement Movement;
	GameManager Manager;
	LineRenderer laser;

	PowerUp.PowerUps CurrentPowerUp;
	private bool HasPowerUp;
	// Use this for initialization
	void Start () {
		Status = GetComponent<PlayerStatus>();
		Movement = GetComponent<CharacterMovement>();
		HasPowerUp = false;
		Manager = GameManager.Instance;
		laser = GetComponent<LineRenderer>();
		laser.enabled = false;
	}

	public bool HasPower()
	{
		return HasPowerUp;
	}

	void LaserSwitch()
	{

	}
	
	IEnumerator LaserWait(Transform t)
	{
		laser.enabled = true;
		laser.SetPosition(0, transform.position);
		laser.SetPosition(1, t.position);
		yield return new WaitForSeconds(0.2f);
		laser.enabled = false;
		Vector3 temp = t.position;
		t.position = gameObject.transform.position;
		gameObject.transform.position = temp;
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
			HasPowerUp = false;
			switch (CurrentPowerUp)
			{
				case PowerUp.PowerUps.Speed:
					Status.SpeedUp();
					break;
				case PowerUp.PowerUps.Grapnel:

					break;
			}
		}
	}
}
