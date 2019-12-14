using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class Row 
{
    public int Id { get; set; }
    public int MapId { get; set; }
    public string Description { get; set; }
    public List<Cell> Cells { get; set; }

    public Row()
    {
        Cells = new List<Cell>();
    }
}