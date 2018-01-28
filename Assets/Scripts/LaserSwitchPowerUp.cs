using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSwitchPowerUp : GenericPowerUp
{

    List<PlayerStatus> OtherPlayers;
	public GameObject Projectile;
	public float Duration;

    public override void SetUp(GameObject player)
    {
        Status = player.GetComponent<PlayerStatus>();
        transform.SetParent(Status.GetHand().transform);
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        Status.SetPowerUp(this);
        PlayerStatus[] temp = FindObjectsOfType<PlayerStatus>();
        OtherPlayers = new List<PlayerStatus>();
        foreach (PlayerStatus ps in temp)
        {
            if (ps != Status)
            {
                OtherPlayers.Add(ps);
            }
        }
        Status.GetPlayerUIPanel().SetPowerUpIcon(iconSprite);
    }

    /*private void Update()
	{
		transform.position = Status.GetHand().transform.position;
	}*/

    public override void Use()
    {
        GameObject target = GetTarget();
        if (target != null)
        {
            Status.GunShoot();
            EnableDisableCommands(target, false);
            StartCoroutine(LaserWait(target.transform));
        }
    }

    GameObject GetTarget()
    {
        GameObject target = null;
        float mindistance = Mathf.Infinity;
        foreach (PlayerStatus ps in OtherPlayers)
        {
            if (!ps.IsDead() /*&& !ps.IsInfected()*/)
            {
                //Check if davanti
                if (IsInSight(ps.transform))
                {
                    float distance = Vector3.Distance(ps.transform.position, transform.parent.position);
                    if (distance < mindistance)
                    {
                        target = ps.gameObject;
                        mindistance = distance;
                    }
                }
            }
        }
        //Debug.Log("Target = " + target);
        return target;
    }

    bool IsInSight(Transform target)
    {
        Vector3 totargetvector = (target.position - transform.parent.position).normalized;
        Vector3 forward = transform.parent.forward;
        float dot = Vector3.Dot(totargetvector, forward);
        //Debug.Log("Dot = " + dot);
        return dot > 0.5f;
    }

    void SwitchPositions(Transform t)
    {
        Vector3 temp = t.position;
        t.position = Status.transform.position;
        Status.transform.position = temp;
    }

    void EnableDisableCommands(GameObject target, bool state)
    {
        //Debug.Log("Enable disable with state " + state);
        GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        target.GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponentInParent<CharacterMovement>().CanMove = state;
        target.GetComponent<CharacterMovement>().CanMove = state;
    }
    IEnumerator LaserWait(Transform t)
    {
        //Debug.Log("Into the coroutine");
		GameObject g = Instantiate(Projectile,transform.position,Quaternion.identity);
		g.GetComponent<SwitchProjectile>().Shoot(t, Duration);
        yield return new WaitForSeconds(Duration);
        SwitchPositions(t);
        EnableDisableCommands(t.gameObject, true);
        Status.GetPlayerUIPanel().SetPowerUpIcon(null);
        Destroy(gameObject);
    }

    public override void SelfDestruct()
    {
        Status.GetPlayerUIPanel().SetPowerUpIcon(null);
        Destroy(gameObject);
    }
}
