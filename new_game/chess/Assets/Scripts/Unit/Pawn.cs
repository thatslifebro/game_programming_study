using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class Pawn : Base_Controller
{
    public bool promotion;
    public bool EnPassant;

    public abstract void ChangeTogo(int swwv);

    public override bool CanMove()
    {
        GameObject temp;
        for (int i = 0; i < 2; i++)
        {

            dest.x = myPosition.x + togo[i].x;
            dest.y = myPosition.y + togo[i].y;
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
            else break;

        }
        for (int i = 2; i < 4; i++)
        {
            dest.x = myPosition.x + togo[i].x;
            dest.y = myPosition.y + togo[i].y;
            if (UnitMap.TryGetValue(dest, out temp))
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

    public void PromotionCheck()
    {
        if(myPosition.y==3 || myPosition.y == -4)
        {
            promotion = true;
        }
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
            if(UnitMap.TryGetValue(new Vector2(target.x, target.y - togo[3].y), out targetUnit))
            {
                if (targetUnit.GetComponent<Base_Controller>().IsPawn)
                {
                    if (targetUnit.GetComponent<Pawn>().EnPassant)
                    {
                        UnitMap.Remove(new Vector2(target.x, target.y - togo[3].y));
                        targetUnit.SetActive(false);
                    }
                }
                
            }
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
                //앙파상설
                if(target.y - myPosition.y==-2 || target.y - myPosition.y == 2)
                {
                    EnPassant = true;
                }
                myPosition = target;
                transform.position = target * interval + offset;
                PromotionCheck();
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
                    PromotionCheck();
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

    public override bool AttackKing()
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



    public override void ShowPath()
    {

        GameObject temp;
        bool blocked = false;

        for (int i = 0; i < 4; i++)
        {
            Vector2 v = togo[i];
            dest.x = myPosition.x + v.x;
            dest.y = myPosition.y + v.y;
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

        //앙파상
        for(int i = 0; i < 2; i++)
        {
            dest.x = myPosition.x -1+2*i;
            dest.y = myPosition.y;
            
            if(UnitMap.TryGetValue(dest, out temp))
            {
                if (temp.GetComponent<Base_Controller>().IsPawn)
                {
                    if (temp.GetComponent<Pawn>().EnPassant)
                    {
                        if (PointerMap.TryGetValue(new Vector2(dest.x, dest.y + togo[3].y) , out temp))
                            temp.SetActive(true);
                    }
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
