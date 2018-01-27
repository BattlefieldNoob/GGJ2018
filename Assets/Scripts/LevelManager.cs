using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	public int sceneIndex;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void LoadOnSceneIndex(){

		SceneManager.LoadScene (sceneIndex);
		Debug.Log (" Loading  scene number " + sceneIndex);		
	}

	public void LoadOnSceneName(string sceneName){
		
		SceneManager.LoadScene (sceneName);
		Debug.Log (" Loading  scene " + sceneName);
	}

	public void CloseGame (){
		Application.Quit ();
		Debug.Log ("Closing the Application");
	}

	public void LoadAfter(float wait){
		Invoke ("LoadOnSceneIndex", wait);
	}

	public void GoToCharactersSelections()
	{
		sceneIndex = 1;
		LoadAfter(1.2f);
	}
}
