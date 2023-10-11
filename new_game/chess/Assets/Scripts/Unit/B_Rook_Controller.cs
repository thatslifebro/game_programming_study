using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Rook_Controller : RookBishopQueen
{
    
    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;
        First = true;
        IsKing = false;
        IsRook = true;
        AmIWhite = false;
        togo.Add(new Vector2(0, 1));
        togo.Add(new Vector2(0, -1));
        togo.Add(new Vector2(1, 0));
        togo.Add(new Vector2(-1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
