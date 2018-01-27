using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public enum Type
    {
        border = 0,
        targeted = 1
    }

    public Type teleporter_type;

    private Transform child;

    public void Start()
    {
        child = transform.GetChild(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (teleporter_type)
        {
            case Type.border: collision.gameObject.transform.position += child.localPosition;
                break;
            case Type.targeted: collision.gameObject.transform.position = child.position;
                break;
        }
    }
}
