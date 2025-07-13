using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject SnakeBodyPrefab;

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private Vector2Int gridPosition;
    private float GridMoveTimer;
    private float GridMoveTimerMax;
    private Direction GridMoveDirection;
    private LevelGrid levelGrid;
    private GameObject SnakeBody;
    private int SnakeBodyCount;
    private List<SnakeMovePosition> snakeMovePositionList;
    private bool snakeAteFood = false;
    private List<SnakeBodyParts> snakeBodyPartList;

    public void SetUp(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10,10);
        GridMoveTimerMax = 0.2f;
        GridMoveTimer = GridMoveTimerMax;
        GridMoveDirection = Direction.Right;
        SnakeBodyCount = 0;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyParts>();
    }

    private void Update()
    {
       HandleInput();
       HandleGridMovement();

        if (snakeAteFood)
        {
            SnakeBodyCount++;
            snakeAteFood = false ;
            CreateSnakeBodyPart();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (GridMoveDirection != Direction.Down)
            {
                GridMoveDirection = Direction.Up;
            }

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GridMoveDirection != Direction.Up)
            {
                GridMoveDirection = Direction.Down;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GridMoveDirection != Direction.Left)
            {
                GridMoveDirection = Direction.Right;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GridMoveDirection != Direction.Right)
            {
                GridMoveDirection = Direction.Left;
            }
        }
    }

    private void HandleGridMovement()
    {
        GridMoveTimer += Time.deltaTime;
        if (GridMoveTimer >= GridMoveTimerMax)
        {
            GridMoveTimer -= GridMoveTimerMax;

            SnakeMovePosition prevSnakeMovePosition = null;
            if(snakeMovePositionList.Count > 0)
            {
                prevSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(prevSnakeMovePosition, gridPosition, GridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (GridMoveDirection)
            {
                default:
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(3, 0);
                    break;

                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-3, 0);
                    break;

                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, 3);
                    break;

                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -3);
                    break;


            }
            gridPosition += gridMoveDirectionVector;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0,0,GetAngleFormVector(gridMoveDirectionVector)+90);

            if (snakeMovePositionList.Count >= SnakeBodyCount + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            //for (int i = 0; i < snakeMovePositionList.Count; i++)
            //{
            //    Vector2Int snakeMovePosition = snakeMovePositionList[i];

            //    SnakeBody = Instantiate(SnakeBodyPrefab);
            //    SnakeBody.transform.position = new Vector3(snakeMovePosition.x, snakeMovePosition.y);
            //    SnakeBody.transform.eulerAngles = new Vector3(0, 0, GetAngleFormVector(GridMoveDirection) + 90);
            //    Destroy(SnakeBody, GridMoveTimerMax);
            //}

            UpdateSnakeBodyParts();

            //levelGrid.SnakeMoved(gridPosition);
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyParts(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {

        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            snakeBodyPartList[i].snakeBodyGameObject.transform.localScale = new Vector3(17, 9, 0);
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
       List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition } ;
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }


    private class SnakeBodyParts
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public GameObject snakeBodyGameObject;
        public SnakeBodyParts(int BodyIndex)
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SnakeBodySprite;
            
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -BodyIndex;
            transform = snakeBodyGameObject.transform;
           
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch(snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: // previously was going up
                    angle = 0;
                    switch (snakeMovePosition.GetPrevDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left: // previously was going Left
                            angle = 0 + 45;
                            break;
                        case Direction.Right: // previously was going right
                            angle = 0 - 45;
                            break;
                    }
                    break;

                case Direction.Down: // previously was going Down
                    angle = 180;
                    switch (snakeMovePosition.GetPrevDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left: // previously was going Left
                            angle = 180-45;
                            break;
                        case Direction.Right: // previously was going right
                            angle = 180+45;
                            break;
                    }
                    break;

                case Direction.Left: // currently going to the Left 
                    angle = -90;
                    switch (snakeMovePosition.GetPrevDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down: // previously was going down
                            angle = -45;
                            break;
                        case Direction.Up: // previously was going up
                            angle = 45;
                            break;
                    }
                    break;

                case Direction.Right: // currently going to the right 
                    angle = 90;
                    switch (snakeMovePosition.GetPrevDirection())
                    {
                        default:
                            angle = 90;
                            break;
                        case Direction.Down: // previously was going down
                            angle = 45;
                            break;
                        case Direction.Up: // previously was going up
                            angle = -45;
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0,0,angle);

        }
    }


    //Handles One Move Position From the Snake
    private class SnakeMovePosition
    {
        private SnakeMovePosition prevSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition prevSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.prevSnakeMovePosition = prevSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPrevDirection()
        {
            if(prevSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return prevSnakeMovePosition.direction;
            }
           
        }
    }
}
