using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake Snake;
    private LevelGrid LevelGrid;
    void Start()
    {
        Debug.Log("GameHandler Start");
        LevelGrid = new LevelGrid(37, 37);

        Snake.SetUp(LevelGrid);
        LevelGrid.SetUp(Snake);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
