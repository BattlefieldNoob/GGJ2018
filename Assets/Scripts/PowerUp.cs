using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private bool active = true;

    private int powerUps = 1;


    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (active)
        {
            active = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            switch (powerUps)
            {
                case 1: target.GetComponent<PlayerStatus>().SpeedUp();
                    break;
                default:
                    break;
            }
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10.0f);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
