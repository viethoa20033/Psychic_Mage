using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerController playerController;

    private Vector2 direction;
    private bool isClick;

    private void Update()
    {
        if (!GameManager.Instance.isPlaying)
        {
            isClick = false;
            return;
        }
        
        
        if (isClick)
        {
            if (!playerController.isMove)
            {
                playerController.directionMove = direction;
            }
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    public void MoveLeft()
    {
        isClick = true;
        if (!playerController.isMove)
        {
            direction = Vector2.left;
        }
    }

    public void MoveRight()
    {
        isClick = true;
        if (!playerController.isMove)
        {
            direction = Vector2.right;
        }
    }

    public void MoveUp()
    {
        isClick = true;
        if (!playerController.isMove)
        {
            direction = Vector2.up;
        }
    }

    public void MoveDown()
    {
        isClick = true;
        if (!playerController.isMove)
        {
            direction = Vector2.down;
        }
    }

    public void ExitMove()
    {
        isClick = false;
    }
}
