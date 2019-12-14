using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public static class MovementHelper
{

    //this method will return what to do in the next movement. Stop if there is no possible moves, or straigh if the next block is valid, or up or down if it has to look for a valid pass

    public static GameCell GetClosestEnemy(int x, int y, GameCell[,] boardState, bool hero)
    {
        for (int range = 1; range < (Helper.maxY / 2) + Helper.maxX; range++)
        {
            var cells = GetCellsInRange(x, y, range, boardState);
            foreach (GameCell cell in cells)
            {
                if (hero && boardState[cell.X, cell.Y].monster || !hero && boardState[cell.X, cell.Y].hero)
                {
                    var gameCell = boardState[cell.X, cell.Y];
                    gameCell.Range = range;
                    return gameCell;
                }
            }
        }
        return null;
    }

    public static List<GameCell> GetCellsInRange(int posX, int posY, int range, GameCell[,] boardState)
    {
        var cells = new List<GameCell>();
        var maxX = Helper.maxX;
        var maxY = Helper.maxY;

        for (int x = 0; x <= posX + range; x++)
        {
            if (x < posX - range || x >= maxX) continue;

            int dif = x - posX;

            if (dif == 0)
            {
                if (posY + range < maxY && posY + range >= 0) cells.Add(new GameCell() { X = x, Y = posY + range });
                if (posY - range < maxY && posY - range >= 0) cells.Add(new GameCell() { X = x, Y = posY - range });
            }
            else if ((dif > 0 && posY % 2 == 0) || (dif < 0 && posY % 2 != 0))
            {
                if (dif == range)
                {
                    if (posY >= 0) cells.Add(new GameCell() { X = x, Y = posY });
                }
                else if (range > dif * 2)
                {
                    if (posY + range < maxY && posY + range >= 0) cells.Add(new GameCell() { X = x, Y = posY + range });
                    if (posY - range < maxY && posY - range >= 0) cells.Add(new GameCell() { X = x, Y = posY - range });
                }
                else
                {
                    var start = ((range - dif) * 2) - 1;
                    var end = start + 1;
                    if (posY + start < maxY && posY + start >= 0) cells.Add(new GameCell() { X = x, Y = posY + start });
                    if (posY - start < maxY && posY - start >= 0) cells.Add(new GameCell() { X = x, Y = posY - start });
                    if (posY + end < maxY && posY + end >= 0) cells.Add(new GameCell() { X = x, Y = posY + end });
                    if (posY - end < maxY && posY - end >= 0) cells.Add(new GameCell() { X = x, Y = posY - end });
                }
                continue;
            }
            else
            {
                if (System.Math.Abs(dif) == range)
                {
                    cells.Add(new GameCell() { X = x, Y = posY });
                    if (posY + (range - 1) < maxY && posY + (range - 1) >= 0) cells.Add(new GameCell() { X = x, Y = posY + (range - 1) });
                    if (posY - (range - 1) < maxY && posY - (range - 1) >= 0) cells.Add(new GameCell() { X = x, Y = posY - (range - 1) });
                }
                else if (range > (System.Math.Abs(dif) * 2) - 1)
                {
                    if (posY + range < maxY && posY + range >= 0) cells.Add(new GameCell() { X = x, Y = posY + range });
                    if (posY - range < maxY && posY - range >= 0) cells.Add(new GameCell() { X = x, Y = posY - range });
                }
                else
                {
                    var start = ((range - dif) * 2);
                    var end = start + 1;
                    if (posY + (range - start) < maxY && posY + (range - start) >= 0) cells.Add(new GameCell() { X = x, Y = posY + (range - start) });
                    if (posY - (range - start) < maxY && posY - (range - start) >= 0) cells.Add(new GameCell() { X = x, Y = posY - (range - start) });
                    if (posY + (range - end) < maxY && posY + (range - end) >= 0) cells.Add(new GameCell() { X = x, Y = posY + (range - end) });
                    if (posY - (range - end) < maxY && posY - (range - end) >= 0) cells.Add(new GameCell() { X = x, Y = posY - (range - end) });
                }
                continue;
            }
        }

        return cells;
    }

    public static GameCell GetNextMovement(Character character, GameCell target, GameCell[,] boardState)
    {
        if (IsPathPassable(character.x, target.X, target.Y, boardState))
        {
            return new GameCell() { X = character.x < target.X ? character.x + 1 : character.x - 1, Y = character.y };
        }
        // CONTINUE HERE
        return new GameCell() { X = character.x, Y = character.y };
    }

    private static bool MoveUp(Character character, GameCell target, GameCell[,] boardState)
    {
        if (IsPathPassable(character.x, target.X, target.Y, boardState))
        {
            return character.y < target.Y;
        }

        var availablePaths = new List<int>();

        for (int row = 0; row < Helper.maxY; row++)
        {
            if (IsPathPassable(character.x, target.X, row, boardState))
            {
                availablePaths.Add(row);
            }
        }

        int closest = availablePaths.Aggregate((x, y) => Math.Abs(x - character.y) < Math.Abs(y - character.y) ? x : y);

        return character.y < closest;

    }

    private static bool IsPathPassable(int fromX, int toX, int y, GameCell[,] boardState)
    {
        for (int x = ((fromX < toX) ? fromX : toX); x <= ((fromX < toX) ? toX : fromX); x++)
        {
            var path = boardState[x, y].Terrain;
            if (path == terrain.lava || path == terrain.water)
            {
                return false;
            }
        }

        return true;
    }

    public static movement GetNextMovement2(int x, int y, bool hero)
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

        if (y < Helper.maxY - 1)
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

        if (y < Helper.maxY - 2)
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

}