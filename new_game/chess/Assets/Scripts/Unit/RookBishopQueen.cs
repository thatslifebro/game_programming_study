using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookBishopQueen : Base_Controller
{
    public override bool CanMove()
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
                if (dest.x < -4 || dest.x > 3 || dest.y < -4 || dest.y > 3) break;

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
                    break;
                    
                }
            }
        }
        return false;
    }

    public override bool Move(Vector2 target)
    {
        ToggleSelected();
        UnshowPath();
        //1. 아무도 없는 곳 2. 있는데 상대팀 3. 있는데 우리(캐슬링)
        GameObject targetUnit;

        //아무도 없는곳 
        if (!UnitMap.TryGetValue(target, out targetUnit))
        {
            UnitMap.Remove(myPosition);
            UnitMap.Add(target, this.gameObject);
            if (MyKingChecked())
            {
                UnitMap.Remove(target);
                UnitMap.Add(myPosition, this.gameObject);
                return false;
            }
            else
            {
                FirstFalse();
                myPosition = target;
                transform.position = target * interval + offset;
                return true;
            }
        }
        else
        {
            //상대팀 공격
            if (targetUnit.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
            {
                UnitMap.Remove(myPosition);
                UnitMap.Remove(target);
                UnitMap.Add(target, this.gameObject);
                if (MyKingChecked())
                {
                    UnitMap.Remove(target);
                    UnitMap.Add(target, targetUnit);
                    UnitMap.Add(myPosition, this.gameObject);
                    return false;
                }
                else
                {
                    FirstFalse();
                    myPosition = target;
                    transform.position = target * interval + offset;
                    targetUnit.SetActive(false);
                    return true;
                }

            }
            //우리팀 
            else
            {
                return false;
            }
        }


    }

    public override bool AttackTarget(Vector2 target)
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

    public override bool AttackKing()
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
    public override void ShowPath()
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

    public override void UnshowPath()
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
                if (PointerMap.TryGetValue(dest, out temp))
                    temp.SetActive(false);
                else break;
            }
        }
    }
    
}
