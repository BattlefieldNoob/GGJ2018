//=====================================================================
// Global Game Jam 2018
// Written by: Giacomo Garoffolo
//=====================================================================
using FMODUnity;
using UnityEngine;

namespace Audio
{
	public class Ambience : MonoBehaviour {

		[SerializeField] [EventRef] private string _ambMainMenu;
	
		private void Start ()
		{
			AudioManager.PlayAudio(_ambMainMenu);
		}
	
	}
}
