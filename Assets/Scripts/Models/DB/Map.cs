using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Map
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public List<Row> Rows { get; set; }

    public Map()
    {
        Rows = new List<Row>();
    }
}