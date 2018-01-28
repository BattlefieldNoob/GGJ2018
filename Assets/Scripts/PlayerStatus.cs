﻿using System.Collections;
using System.Collections.Generic;
using Audio;
using FMODUnity;
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

    private int temp; 



    public int PlayerID {
        set 
        {
            temp = value;
            SetMaterialFromPlayerID();  
        }
        get
        {
            return temp; 
        }
    }

    State CurrentState;
    public float StunTime = 1.0f;
    CharacterMovement Movement;
	public Transform DeseaseSocket;
	public float NormalSpeed,DeseaseSpeed;
	GenericPowerUp PowerUp;
	public GameObject PistolHand;

    private Animator Animator;
	public ParticleSystem SplashParticles;

	PlayerUIPanel UIPanel;

    public SkinnedMeshRenderer body;

	[EventRef] public string PlayerExplosionSfx;
	[EventRef] public string PlayerStunnedSfx;

    // Use this for initialization
    void Start()
    {
        CurrentState = State.Normal;
        Movement = GetComponent<CharacterMovement>();
        Animator = GetComponentInChildren<Animator>();
        CollidedWithPlayer = new CollisionWithPlayerEvent();
		PowerUp = null;

		int id = PlayerID;
		foreach(PlayerUIPanel p in FindObjectsOfType<PlayerUIPanel>())
		{
			if (p.id == id)
				UIPanel = p; 
		}
		UIPanel.Present(); 
    }

    private void SetMaterialFromPlayerID()
    {
        print("player ID to material = "+temp);
        Material[] tempMat = body.materials;
        Material m = GameManager.Instance.GetMaterialFromPlayerID(temp);
        tempMat[0] = m;
        print(m);
        body.materials = tempMat;  
    }

    public PlayerUIPanel GetPlayerUIPanel()
	{
		return UIPanel; 
	}

    // Update is called once per frame
    void Update()
    {
		if(CanUsePowerUp() && Input.GetButtonDown("Button" + Movement.ControllerID))
		{
			PowerUp.Use();
		}
    }

	public GameObject GetHand()
	{
		return PistolHand;
	}

	bool CanUsePowerUp()
	{
		return PowerUp != null && !IsInfected();
	}

	public void SetPowerUp(GenericPowerUp pu)
	{
		if (PowerUp!=null)
		{
			PowerUp.SelfDestruct();
		}
		PowerUp = pu;
	}

    public void Explode()
    {
        //animazione
        CurrentState = State.Dead;
		if (PowerUp)
			PowerUp.SelfDestruct();
		UIPanel.PlayerIsDead();
		gameObject.SetActive(false);
	    AudioManager.PlayOneShotAudio(PlayerExplosionSfx,gameObject);
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
		UIPanel.PlayerIsAlive(); 
    }

    public void Infect()
    {
		UIPanel.PlayerIsInfected(); 
	    SplashParticles.Play(true);
        Animator.SetTrigger("Infecting");
        CurrentState = State.Stunned;
        Movement.CanMove = false;
	    AudioManager.PlayOneShotAudio(PlayerStunnedSfx,gameObject);
        StartCoroutine(Stun());
    }

	public void GunShoot()
	{
		Animator.SetTrigger("GunShoot");
	}

    private IEnumerator Stun()
    {
        yield return new WaitForSeconds(StunTime);
        CurrentState = State.Infected;
        Movement.CanMove = true;
        Animator.SetBool("Infected",true);

        //Riabilito controlli
    }

    private void BecomeHealthy()
    {
		UIPanel.PlayerIsNoMoreInfected(); 
	    Animator.SetBool("Infected",false);
        CurrentState = State.Normal;
    }

    public Transform GetDeseaseSocket()
    {
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