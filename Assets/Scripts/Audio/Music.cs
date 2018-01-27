//=====================================================================
// Global Game Jam 2018
// Written by: Giacomo Garoffolo
//=====================================================================
using FMODUnity;
using UnityEngine;

namespace Audio
{
	public class Music : MonoBehaviour {
	
		[SerializeField] [EventRef] private string _musMainMenu;

		private void Start ()
		{
			AudioManager.PlayAudio(_musMainMenu);
		}
	

	}
}
