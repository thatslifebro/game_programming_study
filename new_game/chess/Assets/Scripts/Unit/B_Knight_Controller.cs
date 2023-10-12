using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Knight_Controller : Knight
{
    
    
    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;
        IsKing = false;
        AmIWhite = false;
        IsPawn = false;
        IsAlive = true;
        togo.Add(new Vector2(1, 2));
        togo.Add(new Vector2(1, -2));
        togo.Add(new Vector2(-1, 2));
        togo.Add(new Vector2(-1, -2));
        togo.Add(new Vector2(2, 1));
        togo.Add(new Vector2(2, -1));
        togo.Add(new Vector2(-2, 1));
        togo.Add(new Vector2(-2, -1));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
