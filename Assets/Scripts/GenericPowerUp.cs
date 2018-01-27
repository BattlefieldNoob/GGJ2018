using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericPowerUp : MonoBehaviour {

	protected PlayerStatus Status;

	public Sprite iconSprite; 
	public abstract void SetUp(GameObject player);
	public abstract void Use();
	public abstract void SelfDestruct();
}
