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
    private int OldPlayer;

    private bool TimerIsActive;

    public float InfectionTimeSeconds = 15f;
    public float InfectionTimeTimer = 0;

    public float InfectPlayerSeconds = 2f;

    public Text debugtext;

    public Animator Animator;

    public GameObject KaboomParticlePrefab;

    void Start()
    {
        Animator = transform.Find("Model").GetComponentInChildren<Animator>();
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
        Instantiate(KaboomParticlePrefab, transform.position,transform.rotation);
        Debug.Log("[" + GetType().Name + "]" + " Kill current player and find another");
        
        TimerIsActive = false;
        transform.SetParent(null);
        players[CurrentPlayerIndex].Explode();
        InfectionTimeTimer = InfectionTimeSeconds;
        InfectNearestPlayer();
    }


    public void InfectRandomPlayer()
    {
        InfectPlayer(players[Random.Range(0, players.Length)],false);
    }

    public void InfectNearestPlayer()
    {
        var alivePlayers = players.Where(player => !player.IsDead()).ToArray();
        if (alivePlayers.Length == 1)
        {
            Debug.Log("Last Player!");
            //il player che sto infettando è l'ultimo
            EventManager.Instance.OnLastPlayerInfectedPerMatch.Invoke(Array.IndexOf(players, alivePlayers[0]));
            TimerIsActive = false;
        }

        var nearestPlayer = alivePlayers
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position)).FirstOrDefault();
        if (nearestPlayer)
            InfectPlayer(nearestPlayer,alivePlayers.Length == 1);
    }

    private void InfectPlayer(PlayerStatus player,bool isLastPlayer)
    {
        Debug.Log("[" + GetType().Name + "]" + " Infecting Player");
        TimerIsActive = false;
        OldPlayer = CurrentPlayerIndex;
        CurrentPlayerIndex = Array.IndexOf(players, player);

        transform.SetParent(null);

        //mi registro all'evento collisione del player
        players[CurrentPlayerIndex].CollidedWithPlayer.AddListener(OnCollisionWithHealtyPlayer);
        
        Animator.SetBool("Flying",true);

        StartCoroutine(ReachPlayer(players[CurrentPlayerIndex].GetDeseaseSocket(), 10, callback:() =>
        {
            Animator.SetTrigger("Attacking");
            Debug.Log("Animation Finished!");
            //mi metto come figlio dell'oggetto
            transform.SetParent(players[CurrentPlayerIndex].GetDeseaseSocket());
            transform.localPosition=Vector3.zero;
            players[CurrentPlayerIndex].Infect();
            Animator.SetBool("Flying",false);
            if (!isLastPlayer)
            {
                TimerIsActive = true; 
            }
        }));
    }

    IEnumerator ReachPlayer(Transform target, float power,Action callback)
    {
        float duration = 1.0f;
        Vector3 startPosition = transform.position;

        float startY = startPosition.y;
        float endY = target.position.y;
        float bezierY = transform.position.y + power;

        for (float t = 0.0f; t <= duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            float y = ((1 - t) * (1 - t) * startY + 2 * (1 - t) * t * bezierY + t * t * endY);
            Vector3 horizontal = Vector3.Lerp(startPosition, target.position, progress);
            transform.rotation = Quaternion.Lerp(transform.rotation,target.rotation,progress);
            
            transform.position = new Vector3(horizontal.x, y, horizontal.z);
            

            yield return null;
        }
        callback(); 
    }

    private void OnCollisionWithHealtyPlayer(PlayerStatus healtyPlayer)
    {
        Debug.Log("[" + GetType().Name + "]" + " Change Infected Player");
        //il padre ha colliso con un player sano, quindi mi sposto su quello
        InfectPlayer(healtyPlayer,false);
    }
}