using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int hp = 3;
    public int damage;
    public int range;
    public int attackSpeed;
    public int speed;
    public int x;
    public int y;
    public bool hero = true;
    private int xFrom; //this from vars are for knowing where it is coming from
    private int yFrom;
    // Start is called before the first frame update
    void Start()
    {
        var block = GameObject.Find(x + "-" + y);
        block.GetComponent<block>().occupied = true;
        transform.position = block.transform.position;
        xFrom = x;
        yFrom = y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdatePosition()
    {
        if (xFrom != x || yFrom != y)
        {
            GameObject.Find("GameState").GetComponent<GameState>().UpdateBoardState(this, xFrom,yFrom);
            var block = GameObject.Find(x + "-" + y);
            if (!block.GetComponent<block>().occupied)
            {
                GameObject.Find(xFrom + "-" + yFrom).GetComponent<block>().occupied = false;
                block.GetComponent<block>().occupied = true;
                transform.position = block.transform.position;
                xFrom = x;
                yFrom = y;
            }
            else
            {
                x = xFrom;
                y = yFrom;
            }
        }
    }

    public void Move()
    {
        if ((x < 13 && hero) || (x > 0 && !hero))
        {
            switch (GetNextMovement(x, y))
            {
                case movement.straight:
                    x = hero ? x + 1 : x - 1;
                    UpdatePosition();
                    break;
                case movement.up:
                    y--;
                    UpdatePosition();
                    break;
                case movement.down:
                    y++;
                    UpdatePosition();
                    break;
                case movement.stop:
                    break;
            }
        }
    }

    //this method will return what to do in the next movement. Stop if there is no possible moves, or straigh if the next block is valid, or up or down if it has to look for a valid pass
    movement GetNextMovement(int x, int y)
    {
        if ((x >= Helper.maxX && hero) || (x <= 0 && !hero)) return movement.stop; //If it is at the end of the board

        var block = GameObject.Find(hero ? (x + 1) + "-" + y : (x - 1) + "-" + y).GetComponent<block>();
        if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
        {
            if (block.occupied) return movement.stop;
            return movement.straight;
        }

        if (y < Helper.maxY)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y + 1) : (x - 1) + "-" + (y + 1)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.down;
            }
        }

        if (y > 0)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y - 1) : (x - 1) + "-" + (y - 1)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.up;
            }
        }

        if (y < Helper.maxY-1)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y + 2) : (x - 1) + "-" + (y + 2)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.down;
            }
        }

        if (y > 1)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y - 2) : (x - 1) + "-" + (y - 2)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.up;
            }
        }

        if (y < Helper.maxY-2)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y + 3) : (x - 1) + "-" + (y + 3)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.down;
            }
        }

        if (y > 3)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y - 3) : (x - 1) + "-" + (y - 3)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.up;
            }
        }

        if (y < Helper.maxY - 3)
        {
            block = GameObject.Find(hero ? (x + 1) + "-" + (y + 3) : (x - 1) + "-" + (y + 3)).GetComponent<block>();
            if ((block.terrain == terrain.ground || block.terrain == terrain.snow))
            {
                if (block.occupied) return movement.stop;
                return movement.down;
            }
        }

        return movement.stop;
    }

    public void Attack()
    {
        // -1 -1    -1 0   -1 1   0 -1   0 1   1 0  
        foreach (var character in FindObjectsOfType<Character>())
        {
            var characterScript = character.GetComponent<Character>();
            if (IsAdjacent(x, y, characterScript.x, characterScript.y, range))
            {
                character.LoseHp(damage);
                break;
            }
        }
    }

    bool IsAdjacent(int x1, int y1, int x2, int y2, int range = 1)
    {
        if (x1 == x2)
        {
            if ((y1 == y2 - 1) || (y1 == y2 + 1)) return true;
        }

        if (x1 == x2 + 1)
        {
            if ((y1 == y2 - 1) || (y1 == y2 + 1) || (y1 == y2)) return true;
        }

        if (x1 == x2 - 1)
        {
            if (y1 == y2) return true;
        }

        return false;
    }

    public void LoseHp(int damage)
    {
        hp = hp - damage;
    }

    public void CheckHp()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            GameObject.Find(x + "-" + y).GetComponent<block>().occupied = false;
        }
    }
}
