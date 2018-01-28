using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchProjectile : MonoBehaviour {

	Transform  End;
	float Duration;

	public void Shoot(Transform e, float d)
	{
		End = e;
		Duration = d;
		transform.DOMove(e.position, d).OnComplete(() => Destroy(gameObject));
	}
}
