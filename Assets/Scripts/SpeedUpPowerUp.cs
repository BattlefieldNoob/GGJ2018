using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpPowerUp : GenericPowerUp
{
	public override void SelfDestruct()
	{
		Status.SetPowerUp(null);
		Destroy(gameObject);
	}

	public override void SetUp(GameObject g)
	{
		transform.SetParent(g.transform);
		Status = GetComponentInParent<PlayerStatus>();
		Status.SetPowerUp(this);
		Status.GetPlayerUIPanel().SetPowerUpIcon(iconSprite); 
	}

	public override void Use()
	{
		Status.GetPlayerUIPanel().SetPowerUpIcon(null);
		Status.SpeedUp();
		Destroy(gameObject);
	}


}
