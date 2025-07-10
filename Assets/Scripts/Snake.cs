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
        GridMoveTimerMax = 1f;
        GridMoveTimer = GridMoveTimerMax;
        GridMoveDirection = new Vector2Int(1, 0);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(GridMoveDirection.y != -1)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = +1;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GridMoveDirection.y != +1)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = -1;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GridMoveDirection.x != -1)
            {
                GridMoveDirection.x = +1;
                GridMoveDirection.y = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GridMoveDirection.x != +1)
            {
                GridMoveDirection.x = -1;
                GridMoveDirection.y = 0;
            }
        }

        GridMoveTimer += Time.deltaTime;
        if(GridMoveTimer >= GridMoveTimerMax)
        {
            gridPosition += GridMoveDirection;
            GridMoveTimer -= GridMoveTimerMax;
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }
}
