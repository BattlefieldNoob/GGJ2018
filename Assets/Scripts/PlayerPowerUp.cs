using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour {

	PlayerStatus Status;
	CharacterMovement Movement;

	// Use this for initialization
	void Start () {
		Status = GetComponent<PlayerStatus>();
		Movement = GetComponent<CharacterMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
