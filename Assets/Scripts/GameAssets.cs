using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance;
    public Sprite SnakeHeadSprite;
    public Sprite FoodSprite;
    public Sprite SnakeBodySprite;

    private void Awake()
    {
        Instance = this;
    }
}
