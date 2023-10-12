using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Pawn_Controller : Pawn
{
    

    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;

        First = true;
        IsKing = false;
        IsRook = false;
        IsPawn = true;
        int swwv = GameObject.Find("UnitController").GetComponent<UnitController>().SWWV;
        AmIWhite = false;
        IsAlive = true;
        promotion = false;
        if (swwv == 0)
        {
            togo.Add(new Vector2(0, 1));
            togo.Add(new Vector2(0, 2));
            togo.Add(new Vector2(-1, 1));
            togo.Add(new Vector2(1, 1));
        }
        else
        {
            togo.Add(new Vector2(0, -1));
            togo.Add(new Vector2(0, -2));
            togo.Add(new Vector2(-1, -1));
            togo.Add(new Vector2(1, -1));
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
