using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class B_King_Controller : King
{

    void Start()
    {
        UnitMap = GameObject.Find("UnitController").GetComponent<UnitController>().UnitMap;
        PointerMap = GameObject.Find("UnitController").GetComponent<UnitController>().PointerMap;
        First = true;
        IsKing = true;
        IsRook = false;
        IsPawn = false;
        AmIWhite = false;
        IsAlive = true;
        togo.Add(new Vector2(0, 1));
        togo.Add(new Vector2(0, -1));
        togo.Add(new Vector2(1, 0));
        togo.Add(new Vector2(-1, 0));
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
