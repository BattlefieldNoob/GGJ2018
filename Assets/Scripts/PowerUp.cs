using System.Collections;
using System.Collections.Generic;
using Audio;
using FMODUnity;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private bool active = false;

    public List<GameObject> PowersList;

    public float StartTime, RespawnTime;

    public GameObject speed;
    public GameObject gun;

    int randomIndex;

    private string PickPowerUpSfx = "event:/SFX/SFX_PowerUp_PickUp";

    private void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(Respawn(StartTime));
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject target = collision.gameObject;
        if (target.tag == "Player")
            if (active && !target.GetComponent<PlayerStatus>().IsInfected())
            {
                AudioManager.PlayOneShotAudio(PickPowerUpSfx,gameObject);
                transform.GetChild(randomIndex).gameObject.SetActive(false);
                active = false;
                GameObject g = Instantiate(PowersList[randomIndex]);
                g.GetComponent<GenericPowerUp>().SetUp(target);
                StartCoroutine(Respawn(RespawnTime));
            }
    }

    private IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);
        randomIndex = Random.Range(0, PowersList.Count);
        transform.GetChild(randomIndex).gameObject.SetActive(true);
        active = true;
    }
}