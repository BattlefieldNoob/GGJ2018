using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
	private string[] joysticks;

	private bool[] joystickIsReady;

	public PlayerReadyStatus[] playersStatus;
	
	List<int> joystickOriginalindexes=new List<int>();

	public Text CanStart;

	private bool gameCanStart = false;
	
	void Start () {
		//prendo la lista dei joystick connessi
		joysticks = Input.GetJoystickNames();

		for (int i = 0; i < joysticks.Length; i++)
		{
			if(!string.IsNullOrEmpty(joysticks[i]))
				joystickOriginalindexes.Add(i);
		}
		
		joystickIsReady=new bool[joystickOriginalindexes.Count];
		
		foreach (var playerinfo in playersStatus)
		{
			playerinfo.Disable();
		}

		//setto tutti i joystick connessi come "non pronti"
		for (int i = 0; i < joystickOriginalindexes.Count; i++)
		{
			joystickIsReady[i] = false;
			playersStatus[i].SetNotReady();
		}
	}
	
	
	void Update () {
		for (int i = 0; i < joystickOriginalindexes.Count; i++)
		{
			if (!joystickIsReady[i] && Input.GetButtonDown("Button" + (joystickOriginalindexes[i]+1)))
			{
				joystickIsReady[i] = true;
				playersStatus[i].SetReady();
				CheckNumberOfPlayers();
			}
		}
	}


	public void CheckNumberOfPlayers()
	{
		if (joystickIsReady.Length>=2 && joystickIsReady.All(playerReady => playerReady))
		{
			Debug.Log("Game can Start!");
			gameCanStart = true;
			StartCoroutine(WaitAndStartGame());
		}
	}

	IEnumerator WaitAndStartGame()
	{
		CanStart.text = "Game Can Start!  3";
		yield return new WaitForSeconds(1f);
		CanStart.text = "Game Can Start!  2";
		yield return new WaitForSeconds(1f);
		CanStart.text = "Game Can Start!  1";
		yield return new WaitForSeconds(1f);
		
		FindObjectOfType<LevelManager>().LoadOnSceneName("Managers");
		
		GameManager.Instance.WaitGameplaySceneAndStartGame(joystickOriginalindexes.ToArray());
	}
	
}
