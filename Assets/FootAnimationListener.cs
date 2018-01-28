using System.Collections;
using System.Collections.Generic;
using Audio;
using FMODUnity;
using UnityEngine;

public class FootAnimationListener : MonoBehaviour
{

	[EventRef] public string footStepSfx;

	public void OnFoot()
	{
		AudioManager.PlayOneShotAudio(footStepSfx,gameObject);
	}
}
