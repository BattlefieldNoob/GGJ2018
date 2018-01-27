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

    private float cooldownTime = 2.0f;

    private bool onCooldown = false;

    public void Start()
    {
        target = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (teleporter_type)
        {
            case Type.border: collision.gameObject.transform.position += target.localPosition;
                break;
            case Type.targeted: collision.gameObject.transform.position = target.position;
                break;
            case Type.jump: if (!onCooldown)
                            {
                                onCooldown = true;
                                collision.gameObject.GetComponent<CharacterMovement>().CanMove = false;
                                StartCoroutine(jump(collision.gameObject, 20));
                                StartCoroutine(Cooldown());
                            }
                break;
            case Type.shootUp: collision.gameObject.GetComponent<CharacterMovement>().CanMove = false; ;
                               StartCoroutine(jump(collision.gameObject, 90));
                break;
        }
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
        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        yield return new WaitForSeconds(cooldownTime);
        gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        onCooldown = false;
    }
}
