using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;
    private float GridMoveTimer;
    private float GridMoveTimerMax;
    private Vector2Int GridMoveDirection;

    private void Awake()
    {
        gridPosition = new Vector2Int(10,10);
        GridMoveTimerMax = 0.6f;
        GridMoveTimer = GridMoveTimerMax;
        GridMoveDirection = new Vector2Int(1, 0);
    }

    private void Update()
    {
       HandleInput();
       HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (GridMoveDirection.y != -1)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = +2;
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GridMoveDirection.y != +1)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = -2;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GridMoveDirection.x != -1)
            {
                GridMoveDirection.x = +2;
                GridMoveDirection.y = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GridMoveDirection.x != +1)
            {
                GridMoveDirection.x = -2;
                GridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement()
    {
        GridMoveTimer += Time.deltaTime;
        if (GridMoveTimer >= GridMoveTimerMax)
        {
            gridPosition += GridMoveDirection;
            GridMoveTimer -= GridMoveTimerMax;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0,0,GetAngleFormVector(GridMoveDirection)+90);
        }
    }

    private float GetAngleFormVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
