using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    public int x;
    public int y;
    public bool occupied;
    public terrain terrain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadValues()
    {
        var sprite = GetComponent<SpriteRenderer>().sprite.name;

        x = int.Parse(Helper.GetXorYFromString(gameObject.name, "-", true));
        y = int.Parse(Helper.GetXorYFromString(gameObject.name, "-", false));

        var renderer = gameObject.GetComponent<SpriteRenderer>();

        // We are assuming if the block has a child, it is a bridge. This is terrible
        if (sprite.Contains("grass") || sprite.Contains("waste") || this.transform.childCount > 0)
        {
            renderer.sortingOrder = 10 + y;
            terrain = terrain.ground;
        }
        else if (sprite.Contains("lava"))
        {
            renderer.sortingOrder = 1 + y;
            gameObject.transform.position += new Vector3(0, -0.18f);
            terrain = terrain.lava;
        }
        else if (sprite.Contains("water"))
        {
            renderer.sortingOrder = 1 + y;
            gameObject.transform.position += new Vector3(0, -0.18f);
            terrain = terrain.water;
        }
        else if (sprite.Contains("snow"))
        {
            renderer.sortingOrder = 10 + y;
            terrain = terrain.snow;
        }
        else
        {
            Debug.Log("Warning, there is a block with an invalid sprite: " + sprite);
        }
    }
}
