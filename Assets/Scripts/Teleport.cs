using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public enum Type
    {
        border = 0,
        targeted = 1,
        jump = 2,
        shootUp = 3
    }

    public Type teleporter_type;

    private Transform target;

    private float cooldownTime = 5.0f;

    private bool onCooldown = false;

    public void Start()
    {
        target = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (teleporter_type)
        {
            case Type.border:   int rand = Random.Range(0, 4);
                                collision.gameObject.transform.position = transform.GetChild(rand).position;
                break;
            case Type.targeted: collision.gameObject.transform.position = target.position;
                break;
            case Type.jump: if (!onCooldown)
                            {
                                onCooldown = true;
                                collision.gameObject.GetComponent<CharacterMovement>().CanMove = false;
                                StartCoroutine(tombinJump());
                                StartCoroutine(jump(collision.gameObject, 20));
                                StartCoroutine(Cooldown());
                            }
                break;
            case Type.shootUp: collision.gameObject.GetComponent<CharacterMovement>().CanMove = false; ;
                               StartCoroutine(jump(collision.gameObject, 90));
                break;
        }
    }

    private IEnumerator tombinJump()
    {
        float duration = 0.1f;
        Vector3 start = gameObject.transform.position;
        Vector3 end = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        float progress = 0.0f;
        for (float t = 0.0f; t <= duration; t += Time.deltaTime)
        {
            progress = t / duration;
            gameObject.transform.position = Vector3.Lerp(start, end, progress);
            yield return null;
        }
        duration = 0.2f;
        for (float t = 0.0f; t <= duration; t += Time.deltaTime)
        {
            progress = t / duration;
            gameObject.transform.position = Vector3.Lerp(end, start, progress);
            yield return null;
        }
        gameObject.transform.position = start;
    }

        private IEnumerator jump(GameObject player, float power)
    {
        float duration = 1.0f;
        Vector3 startPosition = player.transform.position;

        float startY = startPosition.y;
        float endY = target.position.y;
        float bezierY = transform.position.y + power;

        for (float t = 0.0f; t <= duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            float y = ((1 - t) * (1 - t) * startY + 2 * (1 - t) * t * bezierY + t * t * endY);
            Vector3 horizontal = Vector3.Lerp(startPosition, target.position, progress);
            player.transform.position = new Vector3(horizontal.x, y, horizontal.z);

            yield return null;
        }
        player.GetComponent<CharacterMovement>().CanMove = true;
    }

    private IEnumerator Cooldown()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(cooldownTime);
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        onCooldown = false;
    }
}
