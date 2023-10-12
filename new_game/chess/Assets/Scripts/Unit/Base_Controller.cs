using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Base_Controller : MonoBehaviour
{
    public static float interval = 1.045f;
    public static Vector2 offset = new Vector2(0.5225f, 0.5225f);

    public List<Vector2> togo = new List<Vector2>();
    public Vector2 dest = new Vector2();

    public  bool AmIWhite;
    public bool IsKing;
    public bool IsRook;
    public bool IsPawn;
    public bool First;
    public bool IsAlive;

    public Vector2 myPosition;

    public Dictionary<Vector2, GameObject> UnitMap;
    public Dictionary<Vector2, GameObject> PointerMap;

    
    public abstract void ShowPath();
    public abstract void UnshowPath();
    public abstract bool AttackKing();
    public abstract bool AttackTarget(Vector2 target);

    public abstract bool Move(Vector2 target);
    public abstract bool CanMove();

    public void FirstFalse()
    {
        First = false;
    }

    public bool MyKingChecked()
    {
        //King check
        bool answer = false;
        GameObject temp;
        for (int i = 0; i < 64; i++)
        {
            if (UnitMap.TryGetValue(new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4), out temp))
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                {
                    answer = answer || temp.GetComponent<Base_Controller>().AttackKing();
                }
            }
        }

        return answer;
    }

    public void ToggleSelected()
    {
        if (gameObject.GetComponent<Renderer>().material.color == Color.yellow)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        else
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
    
}
