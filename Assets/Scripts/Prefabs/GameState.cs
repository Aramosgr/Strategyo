using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public float gameSpeed = 1f;
    public mapType board = mapType.BigRiver;
    public GameCell[,] boardState { get; set; }

    private bool GameStart = false;
    private int TotalHeroes = 0;
    private int TotalMonsters = 0;

    // Start is called before the first frame update
    void Start()
    {
        SqlHelper.CreateDDBB();
        var map = GameObject.Find("Board").GetComponent<board>().LoadMap(board);
        Helper.maxX = map.X;
        Helper.maxY = map.Y;
        LoadGameState(map);        
        Invoke("Turn", gameSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadGameState(Map map)
    {
        boardState = new GameCell[map.X, map.Y];
        foreach (Row row in map.Rows)
        {
            foreach (Cell cell in row.Cells)
            {               
                boardState[cell.X, cell.Y] = new GameCell()
                {
                    X = cell.X,
                    Y = cell.Y,
                    hero = false,
                    monster = false,
                    occupied = false,
                    bridge = cell.Bridge,
                    Terrain = GameObject.Find(cell.X + "-" + cell.Y).GetComponent<block>().terrain
                };
            } 
        }
    }

    public void UpdateBoardState(Character character, int xFrom, int yFrom)
    {
        if (character.hero)
        {
            boardState[xFrom, yFrom].hero = false;
            boardState[character.x, character.y].hero = true;
        }
        else
        {
            boardState[xFrom, yFrom].monster = false;
            boardState[character.x, character.y].monster = true;
        }
    }

    void Turn()
    {
        var characters = FindObjectsOfType<Character>();

        //MOVEMENT PHASE
        foreach (var character in characters)
        {
            GameCell target = MovementHelper.GetClosestEnemy(character.x,character.y,boardState, character.hero);

            if(target.Range > character.range)
            {
                character.Move();
            }            
        }

        foreach (var character in characters)
        {
            character.Attack();
        }

        foreach (var character in characters)
        {
            character.CheckHp();
        }

        Invoke("Turn", gameSpeed);
    }
}
