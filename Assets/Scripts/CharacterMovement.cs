using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{

	public float Speed;
	public int PlayerID;
	Rigidbody rb;

	public bool CanMove = true;

	private Animator Animator;
	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		Animator = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (CanMove)
		{
			Vector3 dir = new Vector3(Input.GetAxis("HorizontalPL" + PlayerID), 0, Input.GetAxis("VerticalPL" + PlayerID)).normalized;
			if (dir != Vector3.zero)
			{
				transform.rotation = Quaternion.LookRotation(dir);
				rb.velocity = dir * Speed;
			}
		}
		
		Animator.SetFloat("Velocity",rb.velocity.magnitude);
	}
}
