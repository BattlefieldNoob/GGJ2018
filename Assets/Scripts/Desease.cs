using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Desease : MonoBehaviour
{

    private PlayerStatus[] players;
    private int CurrentPlayerIndex;

    private bool TimerIsActive;

    public float InfectionTimeSeconds = 15f;
    public float InfectionTimeTimer = 0;

    public float InfectPlayerSeconds = 2f;

    public Text debugtext;

    void Start()
    {
        players = GameManager.Instance.players.Select(player => player.GetComponent<PlayerStatus>()).ToArray();
		InfectionTimeTimer = InfectionTimeSeconds;
		InfectRandomPlayer();
	}

    void Update()
    {
        if (TimerIsActive)
        {
            if (InfectionTimeTimer > 0)
            {
                InfectionTimeTimer -= Time.deltaTime;

                debugtext.text = InfectionTimeTimer.ToString("##");
            }
            else
            {
                KillCurrentPlayerAndChoseAnother();
            }
        }
    }


    public void StartNewMatch()
    {
        InfectionTimeTimer = InfectionTimeSeconds;
        TimerIsActive = true;
    }

    public void KillCurrentPlayerAndChoseAnother()
    {
        debugtext.text = "Explode!";
        Debug.Log("[" + GetType().Name + "]" + " Kill current player and find another");
        TimerIsActive = false;
        players[CurrentPlayerIndex].Explode();
		InfectionTimeTimer = InfectionTimeSeconds;
		InfectNearestPlayer();
	}


    public void InfectRandomPlayer()
    {
        InfectPlayer(players[Random.Range(0, players.Length)]);
    }

    public void InfectNearestPlayer()
    {
        var alivePlayers = players.Where(player => !player.IsDead()).ToArray();
        if (alivePlayers.Length == 3)
        {
            Debug.Log("Last Player!");
            //il player che sto infettando è l'ultimo
            EventManager.Instance.OnLastPlayerInfectedPerMatch.Invoke(Array.IndexOf(players,alivePlayers[0]));
            TimerIsActive = false;
        }
        
        var nearestPlayer = alivePlayers
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position)).FirstOrDefault();
        if(nearestPlayer)
            InfectPlayer(nearestPlayer);
    }

    private void InfectPlayer(PlayerStatus player)
    {
        Debug.Log("[" + GetType().Name + "]" + " Infecting Player");
		TimerIsActive = false;
		CurrentPlayerIndex = Array.IndexOf(players, player);
        //infetto il player
        players[CurrentPlayerIndex].Infect();

        //mi registro all'evento collisione del player
        players[CurrentPlayerIndex].CollidedWithPlayer.AddListener(OnCollisionWithHealtyPlayer);

        //mi metto come figlio dell'oggetto
        transform.SetParent(players[CurrentPlayerIndex].transform);
        //mi sposto nella stessa posizione del nuovo padre
        transform.DOLocalMove(players[CurrentPlayerIndex].GetDeseaseSocket().localPosition, InfectPlayerSeconds)
            .OnComplete(() =>
            {
                TimerIsActive = true;
            });
    }

    private void OnCollisionWithHealtyPlayer(PlayerStatus healtyPlayer)
    {
        Debug.Log("[" + GetType().Name + "]" + " Change Infected Player");
        //il padre ha colliso con un player sano, quindi mi sposto su quello
        InfectPlayer(healtyPlayer);
    }
}