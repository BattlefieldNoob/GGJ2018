using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    public class CollisionWithPlayerEvent : UnityEvent<PlayerStatus>
    {
    };

    public CollisionWithPlayerEvent CollidedWithPlayer;

    enum State
    {
        Normal,
        Infected,
        Stunned,
        Dead
    };

    State CurrentState;
    private float StunTime = 1.0f;
    CharacterMovement Movement;
	public Transform DeseaseSocket;
	public float NormalSpeed,DeseaseSpeed;

    // Use this for initialization
    void Start()
    {
        CurrentState = State.Normal;
        Movement = GetComponent<CharacterMovement>();
        CollidedWithPlayer = new CollisionWithPlayerEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Infect();
        }
    }

    public void Explode()
    {
        //animazione
        CurrentState = State.Dead;
        gameObject.SetActive(false);
    }

    public bool IsDead()
    {
        return CurrentState == State.Dead;
    }

	public float GetSpeed()
	{
		if (CurrentState == State.Infected)
			return DeseaseSpeed;
		else
			return NormalSpeed;
	}

    public void Resurect()
    {
        if(IsDead())
            CurrentState = State.Normal;
    }

    public void Infect()
    {
        CurrentState = State.Stunned;
        GetComponent<CharacterMovement>().enabled = false;
        StartCoroutine(Stun());
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

    public Transform GetDeseaseSocket()
    {
		//BUG Risolvere!!!!!!!
		return DeseaseSocket;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.transform.GetComponent<PlayerStatus>();
        if (other && CurrentState == State.Infected)
        {
            BecomeHealthy();
            CollidedWithPlayer.Invoke(other);
        }
    }

    public void SpeedUp()
    {
        StartCoroutine(IncreaseSpeed());
    }

    private IEnumerator IncreaseSpeed()
    {
        NormalSpeed += 10;
        yield return new WaitForSeconds(2.0f);
        NormalSpeed -= 10;
    }

    public bool IsInfected()
    {
        return CurrentState == State.Infected;
    }
}