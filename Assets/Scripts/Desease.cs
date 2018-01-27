using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Desease : MonoBehaviour
{
    private PlayerStatus[] _players;
    private int _currentPlayerIndex;

    private bool _timerIsActive;

    public float InfectionTimeSeconds = 15f;
    public float InfectionTimeTimer = 0;

    public float InfectPlayerSeconds = 2f;

    public Text Debugtext;

    public Animator Animator;

    public GameObject KaboomParticlePrefab;

    void Start()
    {
        Animator = transform.Find("Model").GetComponentInChildren<Animator>();
        _players = GameManager.Instance.players.Select(player => player.GetComponent<PlayerStatus>()).ToArray();
        InfectionTimeTimer = InfectionTimeSeconds;
        InfectRandomPlayer();
    }

    void Update()
    {
        if (_timerIsActive)
        {
            if (InfectionTimeTimer > 0)
            {
                InfectionTimeTimer -= Time.deltaTime;

                Debugtext.text = InfectionTimeTimer.ToString("##");
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
        _timerIsActive = true;
    }

    public void KillCurrentPlayerAndChoseAnother()
    {
        Debugtext.text = "";
        Instantiate(KaboomParticlePrefab, transform.position,transform.rotation);
        Debug.Log("[" + GetType().Name + "]" + " Kill current player and find another");
        
        _timerIsActive = false;
        transform.SetParent(null);
        _players[_currentPlayerIndex].Explode();
        InfectionTimeTimer = InfectionTimeSeconds;
        InfectNearestPlayer();
    }


    public void InfectRandomPlayer()
    {
        InfectPlayer(_players[Random.Range(0, _players.Length)],false);
    }

    public void InfectNearestPlayer()
    {
        var alivePlayers = _players.Where(player => !player.IsDead()).ToArray();
        if (alivePlayers.Length == 1)
        {
            Debug.Log("Last Player!");
            //il player che sto infettando è l'ultimo
            EventManager.Instance.OnLastPlayerInfectedPerMatch.Invoke(Array.IndexOf(_players, alivePlayers[0]));
            _timerIsActive = false;
        }

        var nearestPlayer = alivePlayers
            .OrderBy(player => Vector3.Distance(transform.position, player.transform.position)).FirstOrDefault();
        if (nearestPlayer)
            InfectPlayer(nearestPlayer,alivePlayers.Length == 1);
    }

    private void InfectPlayer(PlayerStatus player,bool isLastPlayer)
    {
        Debug.Log("[" + GetType().Name + "]" + " Infecting Player");
        _timerIsActive = false;
        _currentPlayerIndex = Array.IndexOf(_players, player);

        transform.SetParent(null);

        //mi registro all'evento collisione del player
        _players[_currentPlayerIndex].CollidedWithPlayer.AddListener(OnCollisionWithHealtyPlayer);
        
        Animator.SetBool("Flying",true);

        StartCoroutine(ReachPlayer(_players[_currentPlayerIndex].GetDeseaseSocket(), 10, callback:() =>
        {
            Animator.SetTrigger("Attacking");
            Debug.Log("Animation Finished!");
            //mi metto come figlio dell'oggetto
            transform.SetParent(_players[_currentPlayerIndex].GetDeseaseSocket());
            transform.localPosition=Vector3.zero;
            _players[_currentPlayerIndex].Infect();
            Animator.SetBool("Flying",false);

			StartCoroutine(WaitToRestartTimer(isLastPlayer)); 
        }));
    }

	IEnumerator WaitToRestartTimer(bool isLastPlayer)
	{
		if (!isLastPlayer)
		{
			yield return new WaitForSeconds(_players[_currentPlayerIndex].GetComponent<PlayerStatus>().StunTime);
			_timerIsActive = true;
		}
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