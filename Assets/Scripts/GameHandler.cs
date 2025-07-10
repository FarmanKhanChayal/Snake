using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler Start");
        GameObject SnakeHeadGameObject = new GameObject();
        SpriteRenderer SnakeSprteRenderer = SnakeHeadGameObject.AddComponent<SpriteRenderer>();
        SnakeSprteRenderer.sprite = GameAssets.Instance.SnakeHeadSprite;
        SnakeHeadGameObject.transform.localScale = new Vector3(20, 20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
