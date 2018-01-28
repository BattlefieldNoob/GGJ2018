
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

	public List<Material> playerMaterials;


	public TextMeshPro AnotherMatch;
	public TextMeshPro EndGame;

	public Text WhoWonText;

	public Button backButton;

	public List<GameObject> levels;
	
	
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
		StartGame(); 
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			StartGame();
		}
	}

	private void StartGame()
	{
		WhoWonText.text = "";

		foreach (int k in playersControllerIndexes) {
			Debug.Log("At start -> "+k);
		}
		//Instanzio i player
		for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			var player=Instantiate(PlayerPrefab, SpawnPoints[i].position, SpawnPoints[i].rotation);
			players.Add(player);
			//TODO setto alcune impostazioni sui player, tipo colore
			player.GetComponent<CharacterMovement>().ControllerID = playersControllerIndexes[i]+1;
            player.GetComponent<PlayerStatus>().PlayerID = i+1; 
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

    public Material GetMaterialFromPlayerID(int playerID)
    {
        return playerMaterials[playerID - 1]; 
    }

	public void ResetScene()
	{
		foreach (GameObject g in levels)
			g.SetActive(false);
        levels[Random.Range(0, 2)].SetActive(true);
        for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			players[i].transform.position = SpawnPoints[i].position;
			players[i].transform.rotation = SpawnPoints[i].rotation;
			players[i].gameObject.SetActive(true);
			players[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
			players[i].GetComponent<CharacterMovement>().CanMove = false;
		}
	}

	public void Restart()
	{
		for (int i = 0; i < playersControllerIndexes.Length; i++)
		{
			players[i].GetComponent<CharacterMovement>().CanMove = true;
			players[i].GetComponent<PlayerStatus>().Resurect();
		}

		FindObjectOfType<Desease>().StartNewMatch();
	}

	IEnumerator RestartMatchCoroutine(int winner)
	{
		foreach (GenericPowerUp gpu in FindObjectsOfType<GenericPowerUp>())
		{
			gpu.SelfDestruct();
		}

		FindObjectOfType<UFO>().StartTransition();
		yield return null; 
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

		Destroy(FindObjectOfType<Desease>().gameObject);

		foreach (GameObject ps in players)
		{
			Destroy(ps); 
		}

		players.Clear();
		VictoriesPerPlayer.Clear();

		yield return new WaitForSeconds(1); 

		WhoWonText.text = "Player " + winnerOfGame + " won the game!";
		backButton.gameObject.SetActive(true);
	}

}
