using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

abstract public class KnightKing : Base_Controller
{
    public override bool CanMove()
    {
        GameObject temp;
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
            if (dest.x < -4 || dest.x > 3 || dest.y < -4 || dest.y > 3) continue;

            if (UnitMap.TryGetValue(dest, out temp) == false)
            {
                UnitMap.Remove(myPosition);
                UnitMap.Add(dest, this.gameObject);
                if (!MyKingChecked())
                {
                    UnitMap.Add(myPosition, this.gameObject);
                    UnitMap.Remove(dest);
                    return true;
                }
                UnitMap.Add(myPosition, this.gameObject);
                UnitMap.Remove(dest);
            }
            else
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                {
                    UnitMap.Remove(dest);
                    UnitMap.Remove(myPosition);
                    UnitMap.Add(dest, this.gameObject);
                    if (!MyKingChecked())
                    {
                        UnitMap.Remove(dest);
                        UnitMap.Add(dest, temp);
                        UnitMap.Add(myPosition, this.gameObject);
                        return true;
                    }
                    UnitMap.Remove(dest);
                    UnitMap.Add(dest, temp);
                    UnitMap.Add(myPosition, this.gameObject);
                }
            }
        }
        return false;
    }

    public override bool AttackTarget(Vector2 target)
    {

        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
            if (dest == target) return true;

        }
        return false;
    }

    public override bool AttackKing()
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
}
