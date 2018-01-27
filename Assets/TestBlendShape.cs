using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlendShape : MonoBehaviour
{
	private SkinnedMeshRenderer _skinnedMeshRenderer;

	private Mesh mesh;

	public float blendShapeValue;
	// Use this for initialization
	void Start ()
	{
		_skinnedMeshRenderer=GetComponent<SkinnedMeshRenderer>();
		mesh = _skinnedMeshRenderer.sharedMesh;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mesh.blendShapeCount > 0)
		{
			_skinnedMeshRenderer.SetBlendShapeWeight(0, blendShapeValue);
		}
	}
}
