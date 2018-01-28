using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Clouds : MonoBehaviour {

	void Start () {
		foreach (Transform child in transform)
		{
			child.DOMoveX(-30,20f).SetLoops(-1, LoopType.Yoyo).SetDelay(Random.Range(1, 4));
		}
	}
	
}
