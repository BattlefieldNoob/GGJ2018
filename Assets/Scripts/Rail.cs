using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Rail : MonoBehaviour {

	Camera MyCamera;
	public GameObject MenuPosition, GamePosition;
	public float TimeToPosition = 2.0f;

	private void Awake()
	{
		MyCamera = Camera.main;
		SetCameraPosition(MenuPosition);
	}

	void SetCameraPosition(GameObject g)
	{
		MyCamera.transform.position = g.transform.position;
		MyCamera.transform.rotation = g.transform.rotation;
	}

	public void MoveToMenu()
	{
		MoveCamera(MenuPosition); 
	}

	public void MoveToGame()
	{
		MoveCamera(GamePosition);
	}

	void MoveCamera(GameObject g)
	{
		StartCoroutine(MoveCoroutine(g));
	}

	IEnumerator MoveCoroutine(GameObject target)
	{
		MyCamera.transform.DOMove(target.transform.position, TimeToPosition);
		yield return MyCamera.transform.DORotateQuaternion(target.transform.rotation, TimeToPosition).WaitForCompletion();
		
		// Quello che scrivete sotto viene eseguito alla fine dell'animazione
		yield break;
	/*	Vector3 startPos = MyCamera.transform.position;
		Quaternion startRot = MyCamera.transform.rotation;
		float counter = 0;
		while (counter < TimeToPosition)
		{
			float delta = 0.01f;
			counter += delta;
			float alpha = counter / TimeToPosition;
			print(alpha);
			MyCamera.transform.position = Vector3.Lerp(startPos, target.transform.position, alpha);
			MyCamera.transform.rotation = Quaternion.Lerp(startRot, target.transform.rotation, alpha);
			yield return new WaitForSeconds(delta);
		}
		SetCameraPosition(target);*/
	}
}
