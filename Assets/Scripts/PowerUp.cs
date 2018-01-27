using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
