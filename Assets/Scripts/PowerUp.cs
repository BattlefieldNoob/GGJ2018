using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        StartCoroutine(SpeedUp(collision.gameObject));
        
    }

    private IEnumerator SpeedUp(GameObject target)
    {
        target.GetComponent<CharacterMovement>().Speed += 20;
        yield return new WaitForSeconds(2.0f);
        target.GetComponent<CharacterMovement>().Speed -= 20;
    }
}
