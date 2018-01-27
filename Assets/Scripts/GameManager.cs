using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	
	
	#region Singleton

	public static GameManager Instance;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	#endregion

	public float InfectDuration;
	//public Image InfectTimeFilledImage;

	public GameObject PlayerPrefab;
	public GameObject DiseasePrefab;

	public Transform[] SpawnPoints;

	public int[] playersControllerIndexes;

	public PlayerCanvasController CanvasController;
	//public Text canvasText;

	public List<Color> Colors;


	public TextMeshPro AnotherMatch;
	public TextMeshPro EndGame;
	
	
	/// <summary>
	/// Per ogni indice del giocatore tengo traccia del numero di vittorie
	/// </summary>
	public Dictionary<int,int> VictoriesPerPlayer=new Dictionary<int, int>();

	public List<GameObject> players=new List<GameObject>();
	
	void Start () {
		
		EventManager.Instance.OnLastPlayerInfectedPerMatch.AddListener((winner) =>
		{
			print("Event thrown");
			MatchFinished(winner);
		});
	}

	public void WaitGameplaySceneAndStartGame(int[] playersCount)
	{
		playersControllerIndexes = playersCount;
		SceneManager.activeSceneChanged += OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene old, Scene newScene)
	{
		StartGame();
		SceneManager.activeSceneChanged -= OnSceneLoaded;
	}

	private void StartGame()
	{
		foreach (int k in playersControllerIndexes) {
			Debug.Log("At start -> "+k);
		}
		//Instanzio i player
		for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			var player=Instantiate(PlayerPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation);
			players.Add(player);
			//TODO setto alcune impostazioni sui player, tipo colore
			player.GetComponent<CharacterMovement>().PlayerID = playersControllerIndexes[i]+1;
			//inizializzo il numero di vittorie 
			VictoriesPerPlayer.Add(i,0);
		}
		
		//instanzio la Desease
		var desease = Instantiate(DiseasePrefab, Vector3.zero, Quaternion.identity);
		//TODO fare qualcosa sulla desease

		//		CanvasController.InitUI(Colors);
		foreach (string k in Input.GetJoystickNames())
		{
			Debug.Log("Start -> " + k);
		}
	}

	IEnumerator RestartMatchCoroutine(int winner)
	{
		foreach (GenericPowerUp gpu in FindObjectsOfType<GenericPowerUp>())
		{
			gpu.SelfDestruct();
		}

	
		GameObject go = GameObject.Find("Fader");

		yield return new WaitForSeconds(4);
		go.GetComponent<Animator>().SetTrigger("EndRound");
		yield return new WaitForSeconds(2);

		for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			players[i].transform.position=SpawnPoints[i].position;
			players[i].transform.rotation=SpawnPoints[i].rotation;
			players[i].gameObject.SetActive(true);
			players[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
			players[i].GetComponent<CharacterMovement>().CanMove = false;
		}

		go.GetComponent<Animator>().SetTrigger("EndRound");
		yield return new WaitForSeconds(2);


		//TODO aspettare animazione

		for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			players[i].GetComponent<CharacterMovement>().CanMove = true;
			players[i].GetComponent<PlayerStatus>().Resurect();
		}
		
		FindObjectOfType<Desease>().StartNewMatch();
	}

	public void MatchFinished(int winnerOfMatch)
	{
		Debug.Log("Player " + winnerOfMatch + " won this match!");
		//canvasText.text = "Player " + winnerOfMatch + " won this match!";
		//aggiungo un punto vittoria al player
		VictoriesPerPlayer[winnerOfMatch] = VictoriesPerPlayer[winnerOfMatch] + 1;

		if (VictoriesPerPlayer[winnerOfMatch]>=2)
		{
			//WINNER OF THIS GAME!
			StartCoroutine(GameFinished(winnerOfMatch));
		}
		else
		{
			
			StartCoroutine(RestartMatchCoroutine(winnerOfMatch));
		}
	}

	public IEnumerator GameFinished(int winnerOfGame)
	{
		Debug.Log( "Player " + winnerOfGame + " won the game!");
		
		
		yield return new WaitForSeconds(5f);
		EndGame.transform.DOMoveX(-80, 5f).OnComplete(() => { EndGame.transform.DOMoveX(23, 0.01f); });
		yield return new WaitForSeconds(5f);
		
		FindObjectOfType<LevelManager>()?.LoadOnSceneName("Main Menu");

		//canvasText.text = "Player " + winnerOfGame + " won the game!";
	}
	
}
