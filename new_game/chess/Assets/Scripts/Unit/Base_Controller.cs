using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Base_Controller : MonoBehaviour
{
    public  bool AmIWhite;
    public bool IsKing;
    public bool IsRook;
    public bool First;

    public abstract void FirstFalse();
    public abstract void ShowPath(Vector2 chosenPosition, Dictionary<Vector2, GameObject> UnitMap, Dictionary<Vector2, GameObject> PointerMap);
    public abstract void UnshowPath(Vector2 chosenPosition, Dictionary<Vector2, GameObject> UnitMap, Dictionary<Vector2, GameObject> PointerMap);
    public abstract bool AttackKing(Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap);
    public abstract bool AttackTarget(Vector2 target, Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap);

    public void ToggleSelected()
    {
        if (gameObject.GetComponent<Renderer>().material.color == Color.yellow)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        else
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
    // Start is called before the first frame update
    void Start()
    {
        IsKing = false;
        IsRook = false;
        First = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
