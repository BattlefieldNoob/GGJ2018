using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private bool active = false;

	public enum PowerUps { Speed};

	public PowerUps CurrentPowerUp;

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
                target.GetComponent<PlayerPowerUp>().SetPowerUp(CurrentPowerUp);
                StartCoroutine(Respawn());
            }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(6.0f);
		CurrentPowerUp = (PowerUps)Random.Range(0, System.Enum.GetValues(typeof(PowerUps)).Length);
		gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }

    private IEnumerator StartUp()
    {
        yield return new WaitForSeconds(startTime);
		CurrentPowerUp = (PowerUps)Random.Range(0, System.Enum.GetValues(typeof(PowerUps)).Length);
		gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
