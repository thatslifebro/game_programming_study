using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class W_King_Controller : Base_Controller
{
    List<Vector2> togo = new List<Vector2>();
    Vector2 dest = new Vector2();
    bool First = true;
    public override void FirstFalse()
    {
        First = false;
    }
    public override bool AttackKing(Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap)
    {
        GameObject temp;
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
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

        foreach (Vector2 v in togo)
        {
            dest.x = chosenPosition.x + v.x;
            dest.y = chosenPosition.y + v.y;

            if (UnitMap.TryGetValue(dest, out temp) == false)
            {
                if (PointerMap.TryGetValue(dest, out temp))
                    temp.SetActive(true);

            }
            else
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                {
                    if (PointerMap.TryGetValue(dest, out temp))
                    {
                        temp.SetActive(true);
                    }
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
        IsKing = true;
        AmIWhite = true;
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
