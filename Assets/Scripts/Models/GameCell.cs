using UnityEngine;
using System.Collections;

public class GameCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Range { get; set; }
    public terrain Terrain { get; set; }
    public bool bridge { get; set; }
    public bool occupied { get; set; }
    public bool hero { get; set; }
    public bool monster { get; set; }
}
