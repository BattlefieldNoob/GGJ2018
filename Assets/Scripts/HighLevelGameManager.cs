using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLevelGameManager : MonoBehaviour {

	public enum GameState{ Menu, Selection, Game}
	public GameState myState;

	public GameObject menuCanvas;
	public GameObject selectionCanvas;
	public GameObject gameCanvas;

	public Rail rail; 

	private void Awake()
	{
		myState = GameState.Menu; 
	}

	/*private void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			EnterMenuState(); 
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			EnterGameState();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			EnterSelectionState();
		}
	}*/

	public void EnterMenuState()
	{
		rail.MoveToMenu();
		myState = GameState.Menu;
		menuCanvas.SetActive(true);
		selectionCanvas.SetActive(false);
		gameCanvas.SetActive(false);
	}

	public void EnterGameState()
	{
		rail.MoveToGame(); 
		myState = GameState.Game; 
		menuCanvas.SetActive(false);
		selectionCanvas.SetActive(false);
		gameCanvas.SetActive(true);
	}

	public void EnterSelectionState()
	{
		rail.MoveToMenu();
		myState = GameState.Selection;
		menuCanvas.SetActive(false);
		selectionCanvas.SetActive(true);
		gameCanvas.SetActive(false);
	}


}
