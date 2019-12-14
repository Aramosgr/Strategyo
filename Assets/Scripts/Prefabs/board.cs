using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    private mapType mapType;
    public Sprite[] blockSprites;
    public GameObject bridge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Map LoadMap(mapType mapType)
    {
        var map = SqlHelper.GetMap(mapType);

        foreach (Row row in map.Rows)
        {
            foreach (Cell cell in row.Cells)
            {
                var block = GameObject.Find(cell.X + "-" + cell.Y);
                block.GetComponent<SpriteRenderer>().sprite = blockSprites[cell.Sprite];
                if(cell.Bridge)
                {
                    Instantiate(bridge, block.transform.position, Quaternion.identity, block.transform);
                }
                block.GetComponent<block>().LoadValues();
            }
        }
        return map;
    }
}
