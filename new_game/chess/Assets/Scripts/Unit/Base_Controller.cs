using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Base_Controller : MonoBehaviour
{
    public List<Vector2> togo = new List<Vector2>();
    public Vector2 dest = new Vector2();

    public  bool AmIWhite;
    public bool IsKing;
    public bool IsRook;
    public bool First;

    public Dictionary<Vector2, GameObject> UnitMap;
    public Dictionary<Vector2, GameObject> PointerMap;

    

    public abstract void ShowPath(Vector2 chosenPosition);
    public abstract void UnshowPath(Vector2 chosenPosition);
    public abstract bool AttackKing(Vector2 myPosition);
    public abstract bool AttackTarget(Vector2 target, Vector2 myPosition);

    public void FirstFalse()
    {
        First = false;
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
