using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Knight_Controller : Base_Controller
{
    List<Vector2> togo = new List<Vector2>();
    Vector2 dest = new Vector2();

    public override void FirstFalse()
    {
        First = false;
    }

    public override bool AttackTarget(Vector2 target, Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap)
    {
        
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
            if (dest == target) return true;

        }
        return false;
    }

    public override bool AttackKing(Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap)
    {
        GameObject temp;
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x+ v.x;
            dest.y = myPosition.y+ v.y;
            if (UnitMap.TryGetValue(dest, out temp))
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite && temp.GetComponent<Base_Controller>().IsKing)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override void ShowPath(Vector2 chosenPosition, Dictionary<Vector2, GameObject> UnitMap, Dictionary<Vector2, GameObject> PointerMap)
    {

        GameObject temp;

        foreach(Vector2 v in togo)
        {
            dest.x = chosenPosition.x + v.x;
            dest.y = chosenPosition.y + v.y;
            //이동위치
            if (UnitMap.TryGetValue(dest, out temp) == false)
            {
                if (PointerMap.TryGetValue(dest, out temp))
                    temp.SetActive(true);
            }
            //공격 위치
            if (UnitMap.TryGetValue(dest, out temp))
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                {
                    if (PointerMap.TryGetValue(dest, out temp))
                        temp.SetActive(true);
                }
            }
        }
    }

    public override void UnshowPath(Vector2 chosenPosition, Dictionary<Vector2, GameObject> UnitMap, Dictionary<Vector2, GameObject> PointerMap)
    {

        GameObject temp;

        foreach (Vector2 v in togo)
        {
            dest.x = chosenPosition.x + v.x;
            dest.y = chosenPosition.y + v.y;

            if (PointerMap.TryGetValue(dest, out temp))
                temp.SetActive(false);
        }
    }

    void Start()
    {
        IsKing = false;
        AmIWhite = false;
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
