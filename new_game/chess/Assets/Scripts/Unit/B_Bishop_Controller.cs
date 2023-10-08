using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Bishop_Controller : Base_Controller
{
    List<Vector2> togo = new List<Vector2>();
    Vector2 dest = new Vector2();


    public override void FirstFalse() { }
    public override bool AttackKing(Vector2 myPosition, Dictionary<Vector2, GameObject> UnitMap)
    {
        GameObject temp;
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x;
            dest.y = myPosition.y;
            while (true)
            {
                dest.x += v.x;
                dest.y += v.y;
                if (dest.x < -4 || dest.x > 3 || dest.y < -4 || dest.y > 3)
                    break;
                if (UnitMap.TryGetValue(dest, out temp))
                {
                    if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite && temp.GetComponent<Base_Controller>().IsKing)
                    {
                        return true;
                    }
                    else break;
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
            dest.x = chosenPosition.x;
            dest.y = chosenPosition.y;
            while(true)
            {
                dest.x += v.x;
                dest.y += v.y;
                if (UnitMap.TryGetValue(dest, out temp) == false)
                {
                    if (PointerMap.TryGetValue(dest, out temp))
                        temp.SetActive(true);
                    else break;
                }
                else
                {
                    if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                    {
                        if (PointerMap.TryGetValue(dest, out temp))
                        {
                            temp.SetActive(true);
                            break;
                        }
                    }
                    else break;
                }
            }
        }
    }

    public override void UnshowPath(Vector2 chosenPosition, Dictionary<Vector2, GameObject> UnitMap, Dictionary<Vector2, GameObject> PointerMap)
    {

        GameObject temp;

        foreach (Vector2 v in togo)
        {
            dest.x = chosenPosition.x;
            dest.y = chosenPosition.y;
            while (true)
            {
                dest.x += v.x;
                dest.y += v.y;
                if (PointerMap.TryGetValue(dest, out temp))
                    temp.SetActive(false);
                else break;
            }
        }
    }
    void Start()
    {
        IsKing = false;
        AmIWhite = false;
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
