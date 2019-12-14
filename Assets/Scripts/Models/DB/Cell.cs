using UnityEngine;
using UnityEditor;

public class Cell
{
    public int Id { get; set; }
    public int RowId { get; set; }
    public string Description { get; set; }
    public bool Bridge { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Sprite { get; set; }
}