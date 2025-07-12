using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private int height;
    private int width;
    private GameObject foodGamenObject;
    private Snake Snake;
    public LevelGrid(int height, int width)
    {
        this.height = height;
        this.width = width;
    }

    public void SetUp(Snake Snake)
    {
        this.Snake = Snake;

        SpawnFood();
    }

    public void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(-73, width), Random.Range(-37, height));
        }while(Snake.GetFullSNakeMovePositionList().IndexOf(foodGridPosition) != -1);
       

        foodGamenObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGamenObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.FoodSprite;
        foodGamenObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGamenObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        foodGamenObject.AddComponent<Rigidbody2D>().gravityScale = 0;
        foodGamenObject.AddComponent<BoxCollider2D>();

        Debug.Log("food");

    }

    //public void SnakeMoved(Vector2Int snakeGridPosition)
    //{
    //    if(snakeGridPosition == foodGridPosition)
    //    {
    //        Object.Destroy(foodGamenObject);
    //        SpawnFood();
    //    }
    //}
}
