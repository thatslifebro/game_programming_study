using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class King : KnightKing
{
    

    public bool CheckMated()
    {
        if (MyKingChecked())
        {
            return !CanAvoidCheck();
        }
        return false;
    }

    public bool CanAvoidCheck()
    {
        bool answer = false;
        GameObject temp;
        for (int i = 0; i < 64; i++)
        {
            if (UnitMap.TryGetValue(new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4), out temp))
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite == AmIWhite)
                {
                    answer = answer || temp.GetComponent<Base_Controller>().CanMove();
                }
            }
        }

        return answer;
    }


   

    public override bool Move(Vector2 target)
    {
        ToggleSelected();
        UnshowPath();
        //1. 아무도 없는 곳 2. 있는데 상대팀 3. 있는데 우리(캐슬링)
        GameObject targetUnit;

        //아무도 없는곳 
        if(!UnitMap.TryGetValue(target, out targetUnit))
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
                if (MyKingChecked()) {
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
            //캐슬링 
            else
            {
                ToggleSelected();
                UnshowPath();
                if (target.x == -4)
                {
                    UnitMap.Add(new Vector2(myPosition.x - 2, myPosition.y), this.gameObject);
                    UnitMap.Remove(myPosition);
                    UnitMap.Add(new Vector2(myPosition.x - 1, myPosition.y), targetUnit);
                    UnitMap.Remove(target);
                    transform.position = new Vector2(myPosition.x - 2, myPosition.y) * interval + offset;
                    targetUnit.transform.position = new Vector2(myPosition.x - 1, myPosition.y) * interval + offset;
                    Debug.Log("캐슬링");
                }
                else if (target.x == 3)
                {
                    UnitMap.Add(new Vector2(myPosition.x + 2, myPosition.y), this.gameObject);
                    UnitMap.Remove(myPosition);
                    UnitMap.Add(new Vector2(myPosition.x + 1, myPosition.y), targetUnit);
                    UnitMap.Remove(target);
                    transform.position = new Vector2(myPosition.x + 2, myPosition.y) * interval + offset;
                    targetUnit.transform.position = new Vector2(myPosition.x + 1, myPosition.y) * interval + offset;
                    Debug.Log("캐슬링");
                }

                FirstFalse();
                targetUnit.GetComponent<Base_Controller>().FirstFalse();
                return true;
            }
        }
       
        
    }

    public override void ShowPath()
    {

        GameObject temp;

        //castling
        if (First)
        {
            bool left = true;
            bool right = true;
            int i = 1;
            //비었는지 
            while (myPosition.x - i > -4)
            {
                left = left && !UnitMap.TryGetValue(new Vector2(myPosition.x - i, myPosition.y), out temp);
                i++;
            }
            i = 1;
            while (myPosition.x + i < 3)
            {
                right = right && !UnitMap.TryGetValue(new Vector2(myPosition.x + i, myPosition.y), out temp);
                i++;
            }

            // 공격받는지
            Vector2 target;
            if (left)
            {
                bool attacked = false;
                for (int j = 0; j < 3; j++)
                {
                    target = new Vector2(myPosition.x - j, myPosition.y);
                    for (i = 0; i < 64; i++)
                    {
                        Vector2 v = new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4);
                        if (UnitMap.TryGetValue(v, out temp))
                        {
                            if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                            {
                                attacked = attacked || temp.GetComponent<Base_Controller>().AttackTarget(target);
                            }
                        }
                    }
                }
                if (attacked) left = false;

            }

            if (right)
            {
                Debug.Log("right");
                bool attacked = false;
                for (int j = 0; j < 3; j++)
                {
                    target = new Vector2(myPosition.x + j, myPosition.y);
                    for (i = 0; i < 64; i++)
                    {
                        Vector2 v = new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4);
                        if (UnitMap.TryGetValue(v, out temp))
                        {
                            if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                            {
                                attacked = attacked || temp.GetComponent<Base_Controller>().AttackTarget(target);
                            }
                        }
                    }
                }
                if (attacked) right = false;
            }


            //Pointer 변경
            if (left && UnitMap.TryGetValue(new Vector2(-4, myPosition.y), out temp))
            {
                if (temp.GetComponent<Base_Controller>().IsRook && temp.GetComponent<Base_Controller>().First)
                {
                    if (PointerMap.TryGetValue(new Vector2(-4, myPosition.y), out temp))
                    {
                        temp.SetActive(true);
                    }
                }
            }

            if (right && UnitMap.TryGetValue(new Vector2(3, myPosition.y), out temp))
            {
                if (temp.GetComponent<Base_Controller>().IsRook && temp.GetComponent<Base_Controller>().First)
                {
                    if (PointerMap.TryGetValue(new Vector2(3, myPosition.y), out temp))
                    {
                        temp.SetActive(true);
                    }
                }
            }
        }

        //moving
        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;

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

    public override void UnshowPath()
    {

        GameObject temp;
        //castling
        if (PointerMap.TryGetValue(new Vector2(-4, myPosition.y), out temp))
        {
            temp.SetActive(false);
        }
        if (PointerMap.TryGetValue(new Vector2(3, myPosition.y), out temp))
        {
            temp.SetActive(false);
        }

        foreach (Vector2 v in togo)
        {
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
            if (PointerMap.TryGetValue(dest, out temp))
                temp.SetActive(false);
        }
    }
    
}
