using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MovableBlock : MonoBehaviour
{
    public float speed;
    Vector3 startPosition;
    Vector3 targetPosition;
    bool isMoving = false;
    float t;
    bool canMove = true;
    public bool justTeleported;
    public void DisableMove()
    {
        canMove = false;
    }
    public void EnableMove()
    {
        canMove = true;
    }
    private void Update()
    {
        if (isMoving)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (t > 1)
                isMoving = false;
        }
    }
    public void Move(Vector3 move)
    {
        if (!canMove)
            return;
        if (move.magnitude == 0)
            return;
        if (GameManager.Instance.WallInFront(transform.position, move))
        {
            return;
        }
        if (!GameManager.Instance.ValidFloorCheck(transform.position + move))
        {
            return;
        }
        startPosition = transform.position;
        targetPosition = transform.position + move;
        isMoving = true;
        t = 0;
    }
}
