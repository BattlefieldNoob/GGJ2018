using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour {

	public float Speed;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")).normalized;
		if (dir != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(dir);
			rb.velocity = dir * Speed;
		}
	}
}
