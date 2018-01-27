using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
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

	public int NumberOfPlayers = 2;

	public PlayerCanvasController CanvasController;
	//public Text canvasText;

	public List<Color> Colors;
	
	/// <summary>
	/// Per ogni indice del giocatore tengo traccia del numero di vittorie
	/// </summary>
	public Dictionary<int,int> VictoriesPerPlayer=new Dictionary<int, int>();

	public List<GameObject> players=new List<GameObject>();
	
	void Start () {
		
		StartGame();
		
		EventManager.Instance.OnLastPlayerInfectedPerMatch.AddListener((winner) =>
		{
			MatchFinished(winner);
			
		});
	}
	

	public void StartGame()
	{
		//Instanzio i player
		for (int i = 0; i < NumberOfPlayers; i++)
		{
			var player=Instantiate(PlayerPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation);
			players.Add(player);
			//TODO setto alcune impostazioni sui player, tipo colore
			player.GetComponent<CharacterMovement>().PlayerID = i+1;
			//inizializzo il numero di vittorie 
			VictoriesPerPlayer.Add(i,0);
		}
		
		//instanzio la Desease
		var desease = Instantiate(DiseasePrefab, Vector3.zero, Quaternion.identity);
		//TODO fare qualcosa sulla desease
		
		CanvasController.InitUI(Colors);
		
	}

	public void MatchFinished(int winnerOfMatch)
	{
		//canvasText.text = "Player " + winnerOfMatch + " won this match!";
		//aggiungo un punto vittoria al player
		VictoriesPerPlayer[winnerOfMatch] = VictoriesPerPlayer[winnerOfMatch] + 1;

		if (VictoriesPerPlayer[winnerOfMatch]>=2)
		{
			//WINNER OF THIS GAME!
			GameFinished(winnerOfMatch);
		}
	}

	public void GameFinished(int winnerOfGame)
	{
		//canvasText.text = "Player " + winnerOfGame + " won the game!";
	}
}
