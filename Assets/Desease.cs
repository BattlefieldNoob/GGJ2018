using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Desease : MonoBehaviour
{
    private PlayerStatus[] players;

    private int CurrentPlayerIndex;

    private bool TimerIsActive;

    public float InfectionTimeSeconds = 15f;
    public float InfectionTimeTimer = 0;

    public float InfectPlayerSeconds = 2f;


    void Start()
    {
        players = FindObjectsOfType<PlayerStatus>();
        InfectRandomPlayer();
    }

    void Update()
    {
        if (TimerIsActive)
        {
            if (InfectionTimeTimer > 0)
            {
                InfectionTimeTimer -= Time.deltaTime;
            }
            else
            {
                KillCurrentPlayerAndChoseAnother();
            }
        }
    }


    public void KillCurrentPlayerAndChoseAnother()
    {
        Debug.Log("[" + GetType().Name + "]" + " Kill current player and find another");
        TimerIsActive = false;
        players[CurrentPlayerIndex].Explode();
        InfectNearestPlayer();
    }


    public void InfectRandomPlayer()
    {
        InfectPlayer(players[Random.Range(0, players.Length)]);
    }

    public void InfectNearestPlayer()
    {
        var alivePlayers = players.Where(player => !player.IsDead()).ToArray();
        if (alivePlayers.Length == 1)
        {
            //il player che sto infettando è l'ultimo
            EventManager.Instance.OnLastPlayerInfectedPerMatch.Invoke(Array.IndexOf(players,alivePlayers[0]));
        }
        
        var playerIndex = alivePlayers
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position)).First();

            InfectPlayer(playerIndex);
    }

    private void InfectPlayer(PlayerStatus player)
    {
        Debug.Log("[" + GetType().Name + "]" + " Infecting Player");

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
                InfectionTimeTimer = InfectionTimeSeconds;
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