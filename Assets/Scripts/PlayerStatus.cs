using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

	enum State {Normal,Infected,Stunned };
	State CurrentState;
	private float StunTime = 1.0f;
	CharacterMovement Movement;

	// Use this for initialization
	void Start () {
		CurrentState = State.Normal;
		Movement = GetComponent<CharacterMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Infect();
		}
	}

	public void Infect()
	{
		Debug.Log("Infect");
		CurrentState = State.Stunned;
		GetComponent<CharacterMovement>().enabled = false;
		StartCoroutine(Stun());
		//time to wait
		//blendShape
	}

	private IEnumerator Stun()
	{
		yield return new WaitForSeconds(StunTime);
		CurrentState = State.Infected;
		GetComponent<CharacterMovement>().enabled = true;
		//Riabilito controlli
	}

	private void BecomeHealthy()
	{
		CurrentState = State.Normal;
	}

	private void OnCollisionEnter(Collision collision)
	{
		var other = collision.transform.GetComponent<PlayerStatus>();
		if (other && CurrentState == State.Infected)
		{
			BecomeHealthy();
			other.Infect();
		}
	}
}
