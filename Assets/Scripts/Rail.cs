using System.Collections;
using System.Collections.Generic;
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

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			MoveCamera(MenuPosition);
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			MoveCamera(GamePosition);
		}
	}

	public void MoveCamera(GameObject g)
	{
		StartCoroutine(MoveCoroutine(g));
	}

	IEnumerator MoveCoroutine(GameObject target)
	{
		Vector3 startPos = MyCamera.transform.position;
		Quaternion startRot = MyCamera.transform.rotation;
		float counter = 0;
		while (counter < TimeToPosition)
		{
			float delta = Time.deltaTime;
			counter += delta;
			float alpha = counter / TimeToPosition;
			print(alpha);
			MyCamera.transform.position = Vector3.Lerp(startPos, target.transform.position, alpha);
			MyCamera.transform.rotation = Quaternion.Lerp(startRot, target.transform.rotation, alpha);
			yield return new WaitForSeconds(delta);
		}
		SetCameraPosition(target);
	}
}
