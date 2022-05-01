using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerMove : MonoBehaviour
{
    public float speed;
    Vector3 startPosition;
    Vector3 targetPosition;
    bool isMoving = false;
    bool canMove = true;
    float t = 0;
    Animator animator;
    CinemachineImpulseSource impulseSoure;
    public bool justTeleported;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        impulseSoure = GetComponent<CinemachineImpulseSource>();
    }
    void Update()
    {
        if (!isMoving && Input.anyKeyDown && canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            //vertical = Mathf.Clamp01(vertical);
            Move(transform.forward * vertical + transform.right * horizontal);
        }


        if (isMoving)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (t > 1)
                isMoving = false;
        }
    }

    void Move(Vector3 move)
    {
        if (move.magnitude > 1)
            return;
        if (move.magnitude == 0)
            return;
        if(move.z < 0)
        {
            animator.SetTrigger("BadMove");
            StartCoroutine(disableMovement(0.7f));
            return;
        }
        if (GameManager.Instance.WallInFront(transform.position, move))
        {
            GameManager.Instance.HitWall(transform.position,move);
            GameManager.Instance.AddMoveCount();
            StartCoroutine(disableMovement(0.5f));
            StartCoroutine(hitWallAnimation(move));
            return;
        }
        if (!GameManager.Instance.ValidFloorCheck(transform.position + move))
        {
            animator.SetTrigger("BadMove");
            StartCoroutine(disableMovement(0.7f));
            return;
        }
        SoundManager.Instance.PlaySound("PlayerMove");
        startPosition = transform.position;
        targetPosition = transform.position + move;
        isMoving = true;
        t = 0;
        GameManager.Instance.AddMoveCount();
    }
    public IEnumerator disableMovement(float secs)
    {
        canMove = false;
        yield return new WaitForSeconds(secs);
        canMove = true;
    }
    IEnumerator hitWallAnimation(Vector3 move)
    {

        impulseSoure.GenerateImpulse();
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + move/5;
        float t = 0;
        while(t < 0)
        {
            t += Time.deltaTime* 5;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return new WaitForEndOfFrame();
        }
        t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * 5;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return new WaitForEndOfFrame();
        }

    }

    public void TouchInput(Vector3 delta)
    {
        if (isMoving)
            return;
        if (!canMove)
            return;
        Vector3 move = Vector3.zero;
        Vector2 moveAxis = ReturnLargestAxis(delta);
        if(moveAxis.x == delta.x)
        {
            if(moveAxis.x > 0)
            {
                move.x = 1;
            }
            else
            {
                move.x = -1;
            }
        }
        else if(moveAxis.x == delta.y)
        {
            if (moveAxis.x > 0)
            {
                move.z = 1;
            }
        }

        Move(transform.forward * move.z + transform.right * move.x);
    }
    Vector2 ReturnLargestAxis(Vector3 axis)
    {
        Vector2 Largest = Vector2.zero;

        float xValue = Mathf.Abs(axis.x);
        float yValue = Mathf.Abs(axis.y);
        float zValue = Mathf.Abs(axis.z);

        if (xValue > yValue && xValue > zValue)
        {
            Largest.x = axis.x;
            if (yValue > zValue)
                Largest.y = axis.y;
            else Largest.y = axis.z;
        }
        else if (yValue > xValue && yValue > zValue)
        {
            Largest.x = axis.y;
            if (xValue > zValue)
                Largest.y = axis.x;
            else Largest.y = axis.z;
        }
        else if (zValue > xValue && zValue > yValue)
        {
            Largest.x = axis.z;
            if (xValue > yValue)
                Largest.y = axis.x;
            else Largest.y = axis.y;
        }
        else Debug.LogWarning("Sorry, I couldnt figure it out... " + axis.ToString());
        return Largest;

    }

}
