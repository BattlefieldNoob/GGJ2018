using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private bool active = false;

    private int powerUps = 1;

    private int actualPowerUp = 1;

    private float startTime = 3.0f;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(StartUp());
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject target = collision.gameObject;
        if(target.tag == "Player")
            if (active && !target.GetComponent<PlayerStatus>().IsInfected())
            {
                active = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                switch (actualPowerUp)
                {
                    case 1: target.GetComponent<PlayerStatus>().SpeedUp();
                        break;
                }
                StartCoroutine(Respawn());
            }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3.0f);
        actualPowerUp = Random.Range(1, powerUps);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }

    private IEnumerator StartUp()
    {
        yield return new WaitForSeconds(startTime);
        actualPowerUp = Random.Range(1, powerUps);
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
