using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public float InfectDuration;
	public Image InfectTimeFilledImage;

	void Start () {
		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartInfectWave();
		}
	}


	void StartInfectWave()
	{
		InfectTimeFilledImage.fillAmount = 1;
		InfectTimeFilledImage.DOFillAmount(0, InfectDuration).OnComplete(() =>
		{
			EventManager.Instance.OnInfectWaveFinished.Invoke();
			Debug.Log("Start next wave!");
		});
	}
	
}
