using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UnitController : MonoBehaviour
{
    public static float interval = 1.045f;
    public static Vector2 offset = new Vector2(0.5225f,0.5225f);

    public bool StartWithWhiteView;
    public int SWWV;

    Vector2 MousePosition;
    Camera Camera;
    public Tilemap tilemap;

    bool TurnIsWhite = true;

    bool choosing = false;

    GameObject chosenUnit;
    Vector2 chosenUnitPosition;

    public List<GameObject> DeadWhite = new List<GameObject>();
    public List<GameObject> DeadBlack = new List<GameObject>();

    public Dictionary<Vector2, GameObject> UnitMap = new Dictionary<Vector2, GameObject>();
    public Dictionary<Vector2, GameObject> PointerMap = new Dictionary<Vector2, GameObject>();


    void Start()
    {
        
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        StartWithWhiteView = true;
        TurnIsWhite = StartWithWhiteView;
        
        SWWV = 0;
        if (StartWithWhiteView)
            SWWV = 1;

        CreatePointers();
        ResetGame();

        tilemap.SetTileFlags(new Vector3Int(0, 0, 0), TileFlags.None);
        tilemap.SetColor(new Vector3Int(0, 0,0), Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.ScreenToWorldPoint(MousePosition);
            Vector2 clickPosition = new Vector2(tilemap.WorldToCell(MousePosition).x, tilemap.WorldToCell(MousePosition).y);
            GameObject unit;
            GameObject pointer;

            bool clickedUnitOrNot = UnitMap.TryGetValue(clickPosition, out unit);
            bool clickBoard = PointerMap.TryGetValue(clickPosition, out pointer);

            if (!clickBoard) return;
            else ClickBoardHandler(clickedUnitOrNot,unit,pointer,clickPosition);

            
        }
    }

    void ClickBoardHandler(bool clickedUnitOrNot,GameObject unit, GameObject pointer, Vector2 clickPosition)
    {
        //상대방꺼 누르면
        if (clickedUnitOrNot && !choosing && unit.GetComponent<Base_Controller>().AmIWhite != TurnIsWhite)
        {
            Debug.Log("상대꺼누름");
            return;
        }
        //캐슬링
        if (choosing && pointer.activeSelf && clickedUnitOrNot)
        {
            

            if (chosenUnit.GetComponent<Base_Controller>().IsKing && unit.GetComponent<Base_Controller>().IsRook)
            {
                chosenUnit.GetComponent<Base_Controller>().ToggleSelected();
                chosenUnit.GetComponent<Base_Controller>().UnshowPath(chosenUnitPosition);
                if (clickPosition.x == -4)
                {
                    UnitMap.Add(new Vector2(chosenUnitPosition.x - 2, chosenUnitPosition.y), chosenUnit);
                    UnitMap.Remove(chosenUnitPosition);
                    UnitMap.Add(new Vector2(chosenUnitPosition.x - 1, chosenUnitPosition.y), unit);
                    UnitMap.Remove(clickPosition);
                    chosenUnit.transform.position = new Vector2(chosenUnitPosition.x - 2, chosenUnitPosition.y) * interval + offset;
                    unit.transform.position = new Vector2(chosenUnitPosition.x - 1, chosenUnitPosition.y) * interval + offset;
                    Debug.Log("캐슬링");
                }
                else if (clickPosition.x == 3)
                {
                    UnitMap.Add(new Vector2(chosenUnitPosition.x + 2, chosenUnitPosition.y), chosenUnit);
                    UnitMap.Remove(chosenUnitPosition);
                    UnitMap.Add(new Vector2(chosenUnitPosition.x + 1, chosenUnitPosition.y), unit);
                    UnitMap.Remove(clickPosition);
                    chosenUnit.transform.position = new Vector2(chosenUnitPosition.x + 2, chosenUnitPosition.y) * interval + offset;
                    unit.transform.position = new Vector2(chosenUnitPosition.x + 1, chosenUnitPosition.y) * interval + offset;
                    Debug.Log("캐슬링");
                }
               
                chosenUnit.GetComponent<Base_Controller>().FirstFalse();
                unit.GetComponent<Base_Controller>().FirstFalse();
                TurnIsWhite = !TurnIsWhite;
                choosing = false;
                chosenUnit = null;
                return;
            }



        }

        // 고르기 
        if (clickedUnitOrNot && !choosing)
        {
            Debug.Log("고르기");
            choosing = true;
            chosenUnit = unit;
            unit.GetComponent<Base_Controller>().ToggleSelected();
            chosenUnitPosition = clickPosition;
            unit.GetComponent<Base_Controller>().ShowPath(chosenUnitPosition);
        }
        //chosen, 아무것도 없는 곳으로 이동 
        else if (!clickedUnitOrNot && pointer.activeSelf && choosing)
        {
            
            chosenUnit.GetComponent<Base_Controller>().ToggleSelected();
            chosenUnit.GetComponent<Base_Controller>().UnshowPath(chosenUnitPosition);
            UnitMap.Add(clickPosition, chosenUnit);
            UnitMap.Remove(chosenUnitPosition);
            if (Rollback())
            {
                UnitMap.Add(chosenUnitPosition, chosenUnit);
                UnitMap.Remove(clickPosition);
            }
            else
            {
                chosenUnit.transform.position = clickPosition * interval + offset;
                chosenUnit.GetComponent<Base_Controller>().FirstFalse();
                TurnIsWhite = !TurnIsWhite;
                Debug.Log("이동");
            }
            choosing = false;
            chosenUnit = null;
        }
        //chosen, 공격
        else if (clickedUnitOrNot && pointer.activeSelf && choosing)
        {
            
            if (unit.GetComponent<Base_Controller>().AmIWhite != TurnIsWhite)
            {
                chosenUnit.GetComponent<Base_Controller>().FirstFalse();
                UnitMap.Remove(clickPosition);
                UnitMap.Add(clickPosition, chosenUnit);
                UnitMap.Remove(chosenUnitPosition);

                if (Rollback())
                {
                    UnitMap.Add(chosenUnitPosition, chosenUnit);
                    UnitMap.Remove(clickPosition);
                    UnitMap.Add(clickPosition, unit);
                }
                else
                {
                    unit.SetActive(false);
                    chosenUnit.transform.position = clickPosition * interval + offset;
                    TurnIsWhite = !TurnIsWhite;
                    Debug.Log("공격");
                }
            }
            chosenUnit.GetComponent<Base_Controller>().ToggleSelected();
            chosenUnit.GetComponent<Base_Controller>().UnshowPath(chosenUnitPosition);
            choosing = false;
            chosenUnit = null;
        }
        else
        {
            if (choosing)
            {
                chosenUnit.GetComponent<Base_Controller>().ToggleSelected();
            }
            choosing = false;
            if (chosenUnit != null)
                chosenUnit.GetComponent<Base_Controller>().UnshowPath(chosenUnitPosition);
        }

    }

    bool Rollback()
    {
        //King check
        bool answer = false;
        GameObject temp;
        for (int i = 0; i < 64; i++)
        {
            if(UnitMap.TryGetValue(new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f)-4), out temp))
            {
                if (temp.GetComponent<Base_Controller>().AmIWhite != TurnIsWhite)
                {
                    answer = answer || temp.GetComponent<Base_Controller>().AttackKing(new Vector2(i % 8 - 4, (int)System.Math.Truncate((double)i / 8.0f) - 4));
                }
            }
        }

        return answer;
    }

    void ResetGame()
    {
        GameObject B_Pawn_Prefab = Resources.Load<GameObject>("Prefabs/B_Pawn_2D");
        GameObject B_Rook_Prefab = Resources.Load<GameObject>("Prefabs/B_Rook_2D");
        GameObject B_Knight_Prefab = Resources.Load<GameObject>("Prefabs/B_Knight_2D");
        GameObject B_Bishop_Prefab = Resources.Load<GameObject>("Prefabs/B_Bishop_2D");
        GameObject B_Queen_Prefab = Resources.Load<GameObject>("Prefabs/B_Queen_2D");
        GameObject B_King_Prefab = Resources.Load<GameObject>("Prefabs/B_King_2D");
        GameObject W_Pawn_Prefab = Resources.Load<GameObject>("Prefabs/W_Pawn_2D");
        GameObject W_Rook_Prefab = Resources.Load<GameObject>("Prefabs/W_Rook_2D");
        GameObject W_Knight_Prefab = Resources.Load<GameObject>("Prefabs/W_Knight_2D");
        GameObject W_Bishop_Prefab = Resources.Load<GameObject>("Prefabs/W_Bishop_2D");
        GameObject W_Queen_Prefab = Resources.Load<GameObject>("Prefabs/W_Queen_2D");
        GameObject W_King_Prefab = Resources.Load<GameObject>("Prefabs/W_King_2D");

        //B_Pawn
        for (int i = 0; i < 8; i++)
        {
            GameObject B_Pawn = Instantiate(B_Pawn_Prefab);
            B_Pawn.transform.position = new Vector2(-4 + i, -3+5*SWWV) * interval + offset;
            B_Pawn.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            B_Pawn.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-4 + i, -3 + 5 * SWWV), B_Pawn);
        }
        //B_Rook
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Rook = Instantiate(B_Rook_Prefab);
            B_Rook.transform.position = new Vector2(-4 + 7 * i, -4+7*SWWV) * interval + offset;
            B_Rook.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            B_Rook.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-4 + 7 * i, -4+ 7 * SWWV), B_Rook);
        }
        //B_Knight
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Knight = Instantiate(B_Knight_Prefab);
            B_Knight.transform.position = new Vector2(-3 + 5 * i, -4 + 7 * SWWV) * interval + offset;
            B_Knight.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            B_Knight.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-3 + 5 * i, -4 + 7 * SWWV), B_Knight);
        }
        //B_Bishop
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Bishop = Instantiate(B_Bishop_Prefab);
            B_Bishop.transform.position = new Vector2(-2 + 3 * i, -4 + 7 * SWWV) * interval + offset;
            B_Bishop.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            B_Bishop.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-2 + 3 * i, -4 + 7 * SWWV), B_Bishop);
        }
        //B_Queen
        GameObject B_Queen = Instantiate(B_Queen_Prefab);
        B_Queen.transform.position = new Vector2(0-SWWV, -4 + 7 * SWWV) * interval + offset;
        B_Queen.transform.localScale = new Vector3(0.08f, 0.08f, 1);
        B_Queen.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        UnitMap.Add(new Vector2(0 - SWWV, -4 + 7 * SWWV), B_Queen);
        //B_King
        GameObject B_King = Instantiate(B_King_Prefab);
        B_King.transform.position = new Vector2(-1+SWWV, -4 + 7 * SWWV) * interval + offset;
        B_King.transform.localScale = new Vector3(0.08f, 0.08f, 1);
        B_King.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        UnitMap.Add(new Vector2(-1 + SWWV, -4 + 7 * SWWV), B_King);
        //W_Pawn
        for (int i = 0; i < 8; i++)
        {
            GameObject W_Pawn = Instantiate(W_Pawn_Prefab);
            W_Pawn.transform.position = new Vector2(-4 + i, 2-5*SWWV) * interval + offset;
            W_Pawn.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            W_Pawn.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-4 + i, 2 - 5 * SWWV), W_Pawn);
        }
        //W_Rook
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Rook = Instantiate(W_Rook_Prefab);
            W_Rook.transform.position = new Vector2(-4 + 7 * i, 3 - 7 * SWWV) * interval + offset;
            W_Rook.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            W_Rook.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-4 + 7 * i, 3 - 7 * SWWV), W_Rook);
        }
        //W_Knight
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Knight = Instantiate(W_Knight_Prefab);
            W_Knight.transform.position = new Vector2(-3 + 5 * i, 3 - 7 * SWWV) * interval + offset;
            W_Knight.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            W_Knight.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-3 + 5 * i, 3 - 7 * SWWV), W_Knight);
        }
        //W_Wishop
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Bishop = Instantiate(W_Bishop_Prefab);
            W_Bishop.transform.position = new Vector2(-2 + 3 * i, 3 - 7 * SWWV) * interval + offset;
            W_Bishop.transform.localScale = new Vector3(0.08f, 0.08f, 1);
            W_Bishop.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            UnitMap.Add(new Vector2(-2 + 3 * i, 3 - 7 * SWWV), W_Bishop);
        }
        //W_Queen
        GameObject W_Queen = Instantiate(W_Queen_Prefab);
        W_Queen.transform.position = new Vector2(0 - SWWV, 3 - 7 * SWWV) * interval + offset;
        W_Queen.transform.localScale = new Vector3(0.08f, 0.08f, 1);
        W_Queen.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        UnitMap.Add(new Vector2(0 - SWWV, 3 - 7 * SWWV), W_Queen);
        //W_King
        GameObject W_King = Instantiate(W_King_Prefab);
        W_King.transform.position = new Vector2(-1+SWWV, 3 - 7 * SWWV) * interval + offset;
        W_King.transform.localScale = new Vector3(0.08f, 0.08f, 1);
        W_King.transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        UnitMap.Add(new Vector2(-1 + SWWV, 3 - 7 * SWWV), W_King);

        

    }

    void CreatePointers()
    {
        GameObject Pointer_Prefab = Resources.Load<GameObject>("Prefabs/Pointer");
        for (int i=0; i < 64; i++)
        {
            GameObject Pointer = Instantiate(Pointer_Prefab);
            Pointer.transform.position = new Vector2(-4 + i % 8, 3 - (int)System.Math.Truncate((double)i / 8)) * interval + offset;
            Pointer.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            Pointer.transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
            PointerMap.Add(new Vector2(-4 + i % 8, 3 - (int)System.Math.Truncate((double)i / 8)), Pointer);
            Pointer.SetActive(false);
        }
    }

}
