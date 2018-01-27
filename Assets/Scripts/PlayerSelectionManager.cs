using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
	private string[] joysticks;

	private Dictionary<int,bool> joystickIsReady=new Dictionary<int, bool>();

	public PlayerReadyStatus[] playersStatus;
	
	List<int> joystickOriginalindexes=new List<int>();

	public Text CanStart;

	private bool gameCanStart = false;
	
	void Start ()
	{
		StartCoroutine(CheckControllerAvailability());
	}
	
	
	void Update () {
		for (int i = 0; i < joystickOriginalindexes.Count; i++)
		{
			if (joystickIsReady.ContainsKey(joystickOriginalindexes[i]) && !joystickIsReady[joystickOriginalindexes[i]] && Input.GetButtonDown("Button" + (joystickOriginalindexes[i]+1)))
			{
				joystickIsReady[joystickOriginalindexes[i]]=true;
				playersStatus[i].SetReady();
				CheckNumberOfPlayers();
			}
		}
	}


	public void CheckNumberOfPlayers()
	{
		if (joystickIsReady.Count>=2 && joystickIsReady.All(playerReady => playerReady.Value))
		{
			Debug.Log("Game can Start!");
			gameCanStart = true;
			StartCoroutine(WaitAndStartGame());
		}
	}

	IEnumerator CheckControllerAvailability()
	{
		while (true)
		{
			//prendo la lista dei joystick connessi
			joysticks = Input.GetJoystickNames();

			joystickOriginalindexes.Clear();
			
			for (int i = 0; i < joysticks.Length; i++)
			{
				if(!string.IsNullOrEmpty(joysticks[i]))
					joystickOriginalindexes.Add(i);
			}

			//vado a controllare se un joystick con un certo indice non è più collegato e lo rimuovo dalla dictionary di 
			//giocatori pronti
			var keyToRemove = joystickIsReady.Keys.Where(key =>
			{
				return !joystickOriginalindexes.Contains(key);
				
				
			}).ToArray();

			foreach (var key in keyToRemove)
			{
				if(joystickIsReady.ContainsKey(key))
					joystickIsReady.Remove(key);
			}
		
			foreach (var playerinfo in playersStatus)
			{
				playerinfo.Disable();
			}

			Debug.Log("___________________________________");
			Debug.Log(joystickOriginalindexes.Count);
			//setto tutti i joystick connessi come "non pronti"
			for (int i = 0; i < joystickOriginalindexes.Count; i++)
			{
				if(!joystickIsReady.ContainsKey(joystickOriginalindexes[i]))
					joystickIsReady.Add(joystickOriginalindexes[i], false);

				if (joystickIsReady[joystickOriginalindexes[i]])
				{
					Debug.Log("SetReady");
					playersStatus[i].SetReady();
				}
				else
				{
					Debug.Log("NotReady");
					playersStatus[i].SetNotReady();
				}
			}
			
			yield return new WaitForSeconds(2.5f);
			//yield return new WaitForFixedUpdate();
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
		
		FindObjectOfType<LevelManager>().LoadOnSceneName("Gameplay");
		
		GameManager.Instance.WaitGameplaySceneAndStartGame(joystickOriginalindexes.ToArray());
	}
	
}
