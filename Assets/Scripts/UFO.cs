using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{

	public float startingX;
	public float endX; 
	Vector3 startPos;
	Vector3 endPos;
	bool moving;
	public float speed = 2;
	Vector3 halfPos;
	bool done; 

	private void Awake()
	{
		startPos = transform.position;
		startPos.x = startingX;

		endPos = transform.position;
		endPos.x = endX; 

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

			transform.position += dir * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 3, Mathf.Sin(Time.realtimeSinceStartup)/2 )); 

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
                transform.rotation = Quaternion.identity; 
				FindObjectOfType<GameManager>().Restart();

			}
		}
	}
}
