using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

	public int sceneIndex;
	
	#region Singleton

	public static LevelManager Instance;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	#endregion

	private string waitForScene;

	private Image FadePanel;

	public float fadeSeconds=2;


	private void Start()
	{
		FadePanel = transform.GetChild(0).GetComponentInChildren<Image>();
		FadePanel.DOFade(0, fadeSeconds);
		SceneManager.activeSceneChanged+=OnChangeScene;
	}

	private void OnChangeScene(Scene oldScene, Scene newScene)
	{
		if (!string.IsNullOrEmpty(waitForScene))
		{
			if (newScene.name.Contains(waitForScene))
			{
				waitForScene = "";
				// sono sulla scena che attendevo, posso eseguire un FadeIn
				FadePanel.DOFade(0, fadeSeconds);
			}
		}
	}

	public void ChangeSceneTo(string sceneName)
	{
		//Debug.Log("Fading to "+sceneName);
		waitForScene = sceneName;
		FadePanel.DOFade(1, fadeSeconds).OnComplete(() =>
		{
			SceneManager.LoadScene(sceneName);
		});

	}

	public void LoadOnSceneIndex(){

		SceneManager.LoadScene (sceneIndex);
		//Debug.Log (" Loading  scene number " + sceneIndex);		
	}

	public void LoadOnSceneName(string sceneName){
		
		SceneManager.LoadScene (sceneName);
		//Debug.Log (" Loading  scene " + sceneName);
	}

	public void CloseGame (){
		Application.Quit ();
		//Debug.Log ("Closing the Application");
	}

	public void LoadAfter(float wait){
		Invoke ("LoadOnSceneIndex", wait);
	}
}
