using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance;

    [SerializeField]
    private LayerMask groundLayers;

    public LayerMask GroundLayers { get => groundLayers; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool ValidFloorCheck(Vector3 moveTarget)
    {
        RaycastHit hit;
        if (Physics.Raycast(moveTarget, Vector3.down, out hit, 3f, groundLayers))
        {
            MovingFloor movingFloor = hit.transform.GetComponentInParent<MovingFloor>();

            if (movingFloor != null && movingFloor.shouldMove)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public bool WallInFront(Vector3 pos, Vector3 direction)
    {
        if (Physics.Raycast(pos, direction, 1.5f, groundLayers))
        {
            return true;
        }
        return false;
    }

}
