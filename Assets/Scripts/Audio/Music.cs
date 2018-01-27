//=====================================================================
// Global Game Jam 2018
// Written by: Giacomo Garoffolo
//=====================================================================

using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
	public class Music : MonoBehaviour {
	
		[SerializeField] [EventRef] private string _musMainMenu;
		[SerializeField] [EventRef] private string _musGameplay;

		private EventInstance _currentMusicInstance;
		
		private void Awake()
		{
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}


		private void SceneManager_sceneLoaded(Scene currentScene, LoadSceneMode arg1)
		{
			if (currentScene.IsValid())
			{
				AudioManager.StopAudio(_currentMusicInstance);
			}

			switch (currentScene.name)
			{
				case "MainMenu":
					_currentMusicInstance = AudioManager.PlayAudio(_musMainMenu);
					break;
				case "GameScene":
					_currentMusicInstance = AudioManager.PlayAudio(_musGameplay);
					break;
			}
		}



	}
}
