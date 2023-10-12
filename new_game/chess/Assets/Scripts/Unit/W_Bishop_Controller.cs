using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Bishop_Controller : RookBishopQueen
{
    
    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;
        IsKing = false;
        IsPawn = false;
        AmIWhite = true;
        IsAlive = true;
        togo.Add(new Vector2(1, 1));
        togo.Add(new Vector2(1, -1));
        togo.Add(new Vector2(-1, 1));
        togo.Add(new Vector2(-1, -1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
