using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private Vector3 originalPos;

	public void ZoomToPosition(Vector3 pos,float AnimationSeconds)
	{
		originalPos = transform.position;
		transform.DOMove(pos * 0.9f,AnimationSeconds);
	}

	public void ReturnToOriginalPos(float AnimationSeconds)
	{
		transform.DOMove(originalPos, AnimationSeconds);
	}
}
