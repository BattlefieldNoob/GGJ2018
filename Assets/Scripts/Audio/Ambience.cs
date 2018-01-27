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
	public class Ambience : MonoBehaviour
	{

		[SerializeField] [EventRef] private string _ambMainMenu;
		[SerializeField] [EventRef] private string _ambGameplay;

		private EventInstance _currentAmbienceInstance;

		private void Awake()
		{
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}


		private void SceneManager_sceneLoaded(Scene currentScene, LoadSceneMode arg1)
		{


			switch (currentScene.name)
			{
				case "Main Menu":
					if (_currentAmbienceInstance.isValid())
					{
						AudioManager.StopAudio(_currentAmbienceInstance);
					}
					_currentAmbienceInstance = AudioManager.PlayAudio(_ambMainMenu);
					break;
				case "Gameplay":
					if (_currentAmbienceInstance.isValid())
					{
						AudioManager.StopAudio(_currentAmbienceInstance);
					}
					_currentAmbienceInstance = AudioManager.PlayAudio(_ambGameplay);
					break;
			}
		}
	}
}
