using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class King : KnightKing
{
    public override void ShowPath(Vector2 chosenPosition)
    {

        GameObject temp;

        //castling
        if (First)
        {
            bool left = true;
            bool right = true;
            int i = 1;
            //비었는지 
            while (chosenPosition.x - i > -4)
            {
                left = left && !UnitMap.TryGetValue(new Vector2(chosenPosition.x - i, chosenPosition.y), out temp);
                i++;
            }
            i = 1;
            while (chosenPosition.x + i < 3)
            {
                right = right && !UnitMap.TryGetValue(new Vector2(chosenPosition.x + i, chosenPosition.y), out temp);
                i++;
            }

            // 공격받는지
            Vector2 target;
            if (left)
            {
                bool attacked = false;
                for (int j = 0; j < 3; j++)
                {
                    target = new Vector2(chosenPosition.x - j, chosenPosition.y);
                    for (i = 0; i < 64; i++)
                    {
                        Vector2 v = new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4);
                        if (UnitMap.TryGetValue(v, out temp))
                        {
                            if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                            {
                                attacked = attacked || temp.GetComponent<Base_Controller>().AttackTarget(target, v);
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
                    target = new Vector2(chosenPosition.x + j, chosenPosition.y);
                    for (i = 0; i < 64; i++)
                    {
                        Vector2 v = new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4);
                        if (UnitMap.TryGetValue(v, out temp))
                        {
                            if (temp.GetComponent<Base_Controller>().AmIWhite != AmIWhite)
                            {
                                attacked = attacked || temp.GetComponent<Base_Controller>().AttackTarget(target, v);
                            }
                        }
                    }
                }
                if (attacked) right = false;
            }


            //Pointer 변경
            if (left && UnitMap.TryGetValue(new Vector2(-4, chosenPosition.y), out temp))
            {
                if (temp.GetComponent<Base_Controller>().IsRook && temp.GetComponent<Base_Controller>().First)
                {
                    if (PointerMap.TryGetValue(new Vector2(-4, chosenPosition.y), out temp))
                    {
                        temp.SetActive(true);
                    }
                }
            }

            if (right && UnitMap.TryGetValue(new Vector2(3, chosenPosition.y), out temp))
            {
                if (temp.GetComponent<Base_Controller>().IsRook && temp.GetComponent<Base_Controller>().First)
                {
                    if (PointerMap.TryGetValue(new Vector2(3, chosenPosition.y), out temp))
                    {
                        temp.SetActive(true);
                    }
                }
            }
        }

        //moving
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

    public override void UnshowPath(Vector2 chosenPosition)
    {

        GameObject temp;
        //castling
        if (PointerMap.TryGetValue(new Vector2(-4, chosenPosition.y), out temp))
        {
            temp.SetActive(false);
        }
        if (PointerMap.TryGetValue(new Vector2(3, chosenPosition.y), out temp))
        {
            temp.SetActive(false);
        }

        foreach (Vector2 v in togo)
        {
            dest.x = chosenPosition.x + v.x;
            dest.y = chosenPosition.y + v.y;
            if (PointerMap.TryGetValue(dest, out temp))
                temp.SetActive(false);
        }
    }
    
}
