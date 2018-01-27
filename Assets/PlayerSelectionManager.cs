﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
	private string[] joysticks;

	private bool[] joystickIsReady;

	public Text[] playerStatus;

	public Text CanStart;

	private bool gameCanStart = false;
	
	void Start () {
		//prendo la lista dei joystick connessi
		joysticks = Input.GetJoystickNames();
		
		joystickIsReady=new bool[joysticks.Length];

		//setto tutti i joystick connessi come "non pronti"
		for (int i = 0; i < joysticks.Length; i++)
		{
			joystickIsReady[i] = false;
			playerStatus[i].text = "Not Ready!";
		}
		
	}
	
	
	void Update () {
		for (int i = 0; i < joysticks.Length; i++)
		{
			if (!joystickIsReady[i] && Input.GetButtonDown("Button" + (i+1)))
			{
				joystickIsReady[i] = true;
				playerStatus[i].text = "Ready!";
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
		
		GameManager.Instance.StartGame();
	}
	
}
