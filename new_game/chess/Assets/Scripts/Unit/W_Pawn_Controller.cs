using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class W_Pawn_Controller : Pawn
{
    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;
        First = true;
        IsKing = false;
        IsRook = false;
        int swwv = GameObject.Find("UnitController").GetComponent<UnitController>().SWWV;
        AmIWhite = true;
        if (swwv == 1)
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

    void Update()
    {
        
    }
}
