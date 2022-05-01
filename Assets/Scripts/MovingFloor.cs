using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MovingFloor : MonoBehaviour
{
    [SerializeField]
    int moveRequests = 0;
    public Transform floorParent;
    public Transform targetTransform;
    Vector3 spawnPosition;
    Vector3 moveTargetPos;
    Vector3 moveStartPos;

    public bool shouldMove;
    float t;
    public float speed;
    bool atStartPos;
    public MovableBlock blockOnIt;
    private void Start()
    {
        spawnPosition = floorParent.position;
    }
    private void Update()
    {
        if(moveRequests > 0 && atStartPos)
        {
            ChangeState(false);
        }
        else if(moveRequests == 0 && !atStartPos)
        {
            ChangeState(true);
        }
        if(shouldMove)
        {
            t += Time.deltaTime;
            floorParent.transform.position = Vector3.Lerp(moveStartPos, moveTargetPos, t);
            if(t > 1)
            {
                shouldMove = false;
                if(blockOnIt != null)
                    blockOnIt.EnableMove();
            }
        }
    }
    void ChangeState(bool moveAtStart)
    {
        if (moveAtStart)
            moveTargetPos = spawnPosition;
        else
            moveTargetPos = targetTransform.position;

        atStartPos = moveAtStart;
        moveStartPos = floorParent.transform.position;
        t = 0;
        shouldMove = true;
        if(blockOnIt != null)
            blockOnIt.DisableMove();
    }
    public void RequestMoveIn()
    {
        moveRequests++;
    }
    public void RequestMoveBack()
    {
        moveRequests--;
    }
}
