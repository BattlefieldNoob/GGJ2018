using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	public  int sceneIndex;

	public  void LoadOnSceneIndex(){

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
}
