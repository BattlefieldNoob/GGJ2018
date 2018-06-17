using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObject : MonoBehaviour
{
	public GameObject target;
	
	void Start () {
		target.SetActive(false);
	}

	public void Enable()
	{
		target.SetActive(true);
	}
}
