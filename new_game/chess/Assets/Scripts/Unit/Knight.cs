using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : KnightKing
{
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


    public override void ShowPath()
    {

        GameObject temp;

        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
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

    public override void UnshowPath()
    {

        GameObject temp;

        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;

            if (PointerMap.TryGetValue(dest, out temp))
                temp.SetActive(false);
        }
    }

}
