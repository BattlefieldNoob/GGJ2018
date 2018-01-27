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
		transform.DOMove((pos * 0.7f)+new Vector3(0,2,0),AnimationSeconds);
	}

	public void ReturnToOriginalPos(float AnimationSeconds)
	{
		transform.DOMove(originalPos, AnimationSeconds);
	}
}
