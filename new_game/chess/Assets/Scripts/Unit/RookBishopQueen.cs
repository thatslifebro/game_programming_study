using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookBishopQueen : Base_Controller
{

    public override bool AttackTarget(Vector2 target, Vector2 myPosition)
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
                if (dest.x > 3 || dest.x < -4 || dest.y > 3 || dest.y < -4)
                    break;

                if (UnitMap.TryGetValue(dest, out temp) == false)
                {
                    if (dest == target) return true;
                }
                else
                {
                    if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                    {
                        if (dest == target) return true;
                    }
                    else break;
                }
            }
        }
        return false;
    }

    public override bool AttackKing(Vector2 myPosition)
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
    public override void ShowPath(Vector2 chosenPosition)
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

    public override void UnshowPath(Vector2 chosenPosition)
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
    
}
