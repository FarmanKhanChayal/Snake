using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject SnakeBodyPrefab;

    private Vector2Int gridPosition;
    private float GridMoveTimer;
    private float GridMoveTimerMax;
    private Vector2Int GridMoveDirection;
    private LevelGrid levelGrid;
    private GameObject SnakeBody;
    private int SnakeBodyCount;
    private List<Vector2Int> snakeMovePositionList;
    private bool snakeAteFood = false;

    public void SetUp(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10,10);
        GridMoveTimerMax = 0.2f;
        GridMoveTimer = GridMoveTimerMax;
        GridMoveDirection = new Vector2Int(3, 0);
        SnakeBodyCount = 0;
        snakeMovePositionList = new List<Vector2Int>();
    }

    private void Update()
    {
       HandleInput();
       HandleGridMovement();

        if (snakeAteFood)
        {
            SnakeBodyCount++;
            snakeAteFood = false ;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (GridMoveDirection.y != -3)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = +3;
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GridMoveDirection.y != +3)
            {
                GridMoveDirection.x = 0;
                GridMoveDirection.y = -3;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GridMoveDirection.x != -3)
            {
                GridMoveDirection.x = +3;
                GridMoveDirection.y = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GridMoveDirection.x != +3)
            {
                GridMoveDirection.x = -3;
                GridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement()
    {
        GridMoveTimer += Time.deltaTime;
        if (GridMoveTimer >= GridMoveTimerMax)
        {
            GridMoveTimer -= GridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += GridMoveDirection;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0,0,GetAngleFormVector(GridMoveDirection)+90);

            if (snakeMovePositionList.Count >= SnakeBodyCount + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];

                SnakeBody = Instantiate(SnakeBodyPrefab);
                SnakeBody.transform.position = new Vector3(snakeMovePosition.x, snakeMovePosition.y);
                SnakeBody.transform.eulerAngles = new Vector3(0, 0, GetAngleFormVector(GridMoveDirection) + 90);
                Destroy(SnakeBody, GridMoveTimerMax);
            }

            //levelGrid.SnakeMoved(gridPosition);
        }
    }

    private float GetAngleFormVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetSnakeGridPosition()
    {
        return gridPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.name == "Food")
        {
            Destroy(collision.gameObject);
            levelGrid.SpawnFood();
            snakeAteFood = true;
        }
    }

    public List<Vector2Int> GetFullSNakeMovePositionList()
    {
        return snakeMovePositionList;
    }
}
