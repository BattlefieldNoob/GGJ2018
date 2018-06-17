using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineRoomCanvasManager : MonoBehaviour
{


	[Header("Buttons")] 
	[SerializeField] private Button Back;
	[SerializeField] private Button Host;
	[SerializeField] private Button Join;
	[SerializeField] private Button Refresh;

	[Header("RoomList")] 
	[SerializeField] private VerticalLayoutGroup RoomListLayout;


	[Header("Prefabs")] 
	[SerializeField] private RoomEntry RoomEntryPrefab;
	
	private void Awake()
	{
	}

	public void AddRoomEntry(int number, string name, int players)
	{
		Instantiate(RoomEntryPrefab.gameObject,RoomListLayout.transform).GetComponent<RoomEntry>().Initialize(number,name,players);
	}
}
