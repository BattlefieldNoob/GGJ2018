using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{

	public Vector3 startPos;
	public Vector3 endPos;
	bool moving;
	public float speed = 2;
	Vector3 halfPos;
	bool done; 

	private void Awake()
	{
		transform.position = startPos;
		halfPos = (endPos + startPos) / 2;
	}

	public void StartTransition()
	{
		moving = true;
	}

	private void Update()
	{
		if (moving)
		{
			print("moving " + transform.position);
			Vector3 dir = endPos - startPos;
			dir.Normalize();

;
			transform.position += dir * speed * Time.deltaTime;

			if (transform.position.x >= halfPos.x && !done)
			{
				done = true; 
				FindObjectOfType<GameManager>().ResetScene();
			}

			if(transform.position.x >= endPos.x)
			{
				done = false;
				moving = false;
				transform.position = startPos;
				FindObjectOfType<GameManager>().Restart();

			}
		}
	}
}
