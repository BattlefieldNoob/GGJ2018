using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public enum Type
    {
        upper = 0,
        lower = 1,
        right = 2,
        left = 3
    }

    public Type Border;

    public float width;

    public float height;

    private List<Vector3> axis = new List<Vector3>();


    public void Start()
    {
        axis =  new List<Vector3>
        {
            new Vector3( 0, 0, height),
            new Vector3( 0, 0, -height),
            new Vector3( width, 0, 0),
            new Vector3( -width, 0, 0),
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 pos = collision.gameObject.transform.position;
        switch (Border)
        {
            case Type.upper: collision.gameObject.transform.position += axis[0];
                break;
            case Type.lower: collision.gameObject.transform.position += axis[1];
                break;
            case Type.right: collision.gameObject.transform.position += axis[2];
                break;
            case Type.left: collision.gameObject.transform.position += axis[3];
                break;
        }
    }
}
