using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport target;
    public Transform teleportPosition;
    GameObject obj;
    public void TeleportTo(Transform obj)
    {
        SoundManager.Instance.PlaySound("Teleport");
        StartCoroutine(tp(obj));
    }
    IEnumerator tp(Transform ob)
    {
        MovableBlock b = ob.GetComponent<MovableBlock>();
        //Debug.Log("teleported: " + b.justTeleported);
        if(b != null)
        {
            if (b.justTeleported)
                yield break;
            b.DisableMove();
            b.justTeleported = true;
        }
        PlayerMove pm = ob.GetComponent<PlayerMove>();
       // Debug.Log(pm.justTeleported);
        if(pm != null)
        {
            if (pm.justTeleported)
                yield break;
            pm.justTeleported = true;
            StartCoroutine(pm.disableMovement(2.2f));
        }
        if (pm == null && b == null)
            yield break;

        ob.GetComponent<Animator>().SetTrigger("Teleport");
        yield return new WaitForSeconds(1f);
        ob.position = teleportPosition.position;

        yield return new WaitForSeconds(0.2f);

        if (pm != null)
        {
            pm.justTeleported = false;
        }

        if(b!=null)
        {
            b.EnableMove();
            b.justTeleported = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(target.CanTeleport())
        {
            target.TeleportTo(other.transform);
        }
        obj = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == obj)
            obj = null;
    }
    public bool CanTeleport()
    {
        return (obj == null);
    }
}
