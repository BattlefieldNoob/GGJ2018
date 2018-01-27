using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private bool active = false;
	
	public List<GameObject> PowersList;

	public float StartTime, RespawnTime;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(Respawn(StartTime));
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject target = collision.gameObject;
        if(target.tag == "Player")
            if (active && !target.GetComponent<PlayerStatus>().IsInfected())
            {
                active = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
				int randomIndex = Random.Range(0,PowersList.Count);
				GameObject g= Instantiate(PowersList[randomIndex]);
				g.GetComponent<GenericPowerUp>().SetUp(target);
				StartCoroutine(Respawn(RespawnTime));
            }
    }

    private IEnumerator Respawn( float time)
    {
        yield return new WaitForSeconds(time);
		gameObject.GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
