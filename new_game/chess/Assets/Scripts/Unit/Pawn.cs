using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Pawn : Base_Controller
{
    public override bool AttackTarget(Vector2 target, Vector2 myPosition)
    {
        for (int i = 2; i < 4; i++)
        {
            Vector2 v = togo[i];
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;

            if (dest == target)
            {
                return true;
            }
        }
        return false;
    }

    public override bool AttackKing(Vector2 myPosition)
    {
        GameObject temp;
        for (int i = 2; i < 4; i++)
        {
            Vector2 v = togo[i];
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



    public override void ShowPath(Vector2 chosenPosition)
    {

        GameObject temp;
        bool blocked = false;

        for (int i = 0; i < 4; i++)
        {
            Vector2 v = togo[i];
            dest.x = chosenPosition.x + v.x;
            dest.y = chosenPosition.y + v.y;
            if (UnitMap.TryGetValue(dest, out temp) == false && (i == 0 || (First && i == 1 && !blocked)))
            {

                if (PointerMap.TryGetValue(dest, out temp))
                    temp.SetActive(true);

            }
            else if (UnitMap.TryGetValue(dest, out temp) == true && i != 1)
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                {
                    if (PointerMap.TryGetValue(dest, out temp))
                        temp.SetActive(true);
                }
                else if (i == 0)
                {
                    blocked = true;
                }
            }
        }

    }

    public override void UnshowPath(Vector2 chosenPosition)
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
}
