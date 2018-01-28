using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
	PlayerStatus Status;
	public int PlayerID;
	Rigidbody rb;

	public bool CanMove = true;

    public bool CanDash = true;

	private Animator Animator;
	public ParticleSystem WalkParticle;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		Animator = GetComponentInChildren<Animator>();
		Status = GetComponent<PlayerStatus>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
        // TEST DASH BRUH
        if (CanDash && CanMove)
        {
            CanMove = false;
            Vector3 dash = new Vector3(Input.GetAxis("HorizontalPR" + PlayerID), 0, Input.GetAxis("VerticalPR" + PlayerID)).normalized;
            if (dash != Vector3.zero)
            {
                CanDash = false;
                StartCoroutine(dashMove(dash));
                StartCoroutine(dashCooldown());
            }
            else
            {
                CanMove = true;
            }
        }
        if (CanMove)
		{
			Vector3 dir = new Vector3(Input.GetAxis("HorizontalPL" + PlayerID), 0, Input.GetAxis("VerticalPL" + PlayerID)).normalized;
			if (dir != Vector3.zero)
			{
				transform.rotation = Quaternion.LookRotation(dir);
				Vector3 speed = ((new Vector3(dir.x, 0.0f, dir.z)) * Status.GetSpeed()) + new Vector3(0.0f,rb.velocity.y,0.0f);
				rb.velocity = speed;
				if(!WalkParticle.isPlaying)
					WalkParticle.Play(true);
			}
			else
			{
				if(WalkParticle.isPlaying)
					WalkParticle.Stop(true,ParticleSystemStopBehavior.StopEmitting);
			}
        }
		Animator.SetFloat("Velocity",rb.velocity.magnitude/Status.GetSpeed());
	}

    private IEnumerator dashMove(Vector3 dash)
    {
	    Animator.SetBool("Dash",true);
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(dash);
        rb.AddForce(dash * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;
        CanMove = true;
	    Animator.SetBool("Dash",false);
    }

    private IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(2.0f);
        CanDash = true;
    }
}
