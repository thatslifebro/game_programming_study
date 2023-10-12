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
    public int turn;
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

    public GameObject B_King;
    public GameObject W_King;

    public GameObject PromotionBoardBlack;
    public GameObject PromotionBoardWhite;

    public bool promotion;
    public GameObject EnPaccantUnit;
    public bool enPaccant;

    void Start()
    {
        turn = 1;
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        StartWithWhiteView = true;
        TurnIsWhite = StartWithWhiteView;
        promotion = false;
        enPaccant = false;
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

            
            if (promotion)
            {
                if(clickPosition.x<7 && clickPosition.x>4 && clickPosition.y>-2&&clickPosition.y<2)
                    PromotionHandler(clickPosition);
            }
            else if(clickBoard) ClickBoardHandler(clickedUnitOrNot,unit,pointer,clickPosition);

            
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
        // 고르기 
        if (clickedUnitOrNot && !choosing)
        {
            choosing = true;
            chosenUnit = unit;
            unit.GetComponent<Base_Controller>().ToggleSelected();
            chosenUnitPosition = clickPosition;
            unit.GetComponent<Base_Controller>().ShowPath();
        }
        //유닛 움직이기
        else if (choosing && pointer.activeSelf)
        {
            if (chosenUnit.GetComponent<Base_Controller>().Move(clickPosition))
            {
                chosenUnitPosition = clickPosition;
                if (chosenUnit.GetComponent<Base_Controller>().IsPawn)
                {
                    promotion = chosenUnit.GetComponent<Pawn>().promotion;
                    if(promotion == true)
                    {
                        if(TurnIsWhite)
                            PromotionBoardWhite.SetActive(true);
                        else
                            PromotionBoardBlack.SetActive(true);
                        return;
                    }

                    if (enPaccant == true)
                    {
                        EnPaccantUnit.GetComponent<Pawn>().EnPassant = false;
                        enPaccant = false;
                    }

                    //움직인 놈이 앙파상 걸릴 수도 있다면 
                    if (chosenUnit.GetComponent<Pawn>().EnPassant)
                    {
                        EnPaccantUnit = chosenUnit;
                        enPaccant = true;
                    }
                }
                TurnIsWhite = !TurnIsWhite;
            }
            choosing = false;
            
        }
        //잘못 선택  
        else
        {
            if (choosing)
            {
                chosenUnit.GetComponent<Base_Controller>().ToggleSelected();
                chosenUnit.GetComponent<Base_Controller>().UnshowPath();
            }
            choosing = false;
            
        }

        //Game 끝나는지 
        if (!TurnIsWhite)
        {
            if (B_King.GetComponent<King>().CheckMated())
            {
                Debug.Log("White Win");
            }
            //Stalemate
            else if (!B_King.GetComponent<King>().CanAvoidCheck())
            {
                Debug.Log("Draw");
            }
        }
        else
        {
            if (W_King.GetComponent<King>().CheckMated())
            {
                Debug.Log("Black Win");
            }
            //Stalemate
            else if (!W_King.GetComponent<King>().CanAvoidCheck())
            {
                Debug.Log("Draw");
            }
        }


    }

    void PromotionHandler(Vector2 clickPosition)
    {
        if (!TurnIsWhite)
        {
            switch (clickPosition)
            {
                case Vector2 v when v.Equals(new Vector2(5,0)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject B_Knight_Prefab = Resources.Load<GameObject>("Prefabs/B_Knight_2D");
                    GameObject B_Knight = Instantiate(B_Knight_Prefab);
                    B_Knight.transform.position = chosenUnit.transform.position;
                    B_Knight.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, B_Knight);
                    break;
                case Vector2 v when v.Equals(new Vector2(6, 0)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject B_Bishop_Prefab = Resources.Load<GameObject>("Prefabs/B_Bishop_2D");
                    GameObject B_Bishop = Instantiate(B_Bishop_Prefab);
                    B_Bishop.transform.position = chosenUnit.transform.position;
                    B_Bishop.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, B_Bishop);
                    break;
                case Vector2 v when v.Equals(new Vector2(5, -1)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject B_Rook_Prefab = Resources.Load<GameObject>("Prefabs/B_Rook_2D");
                    GameObject B_Rook = Instantiate(B_Rook_Prefab);
                    B_Rook.transform.position = chosenUnit.transform.position;
                    B_Rook.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, B_Rook);
                    break;
                case Vector2 v when v.Equals(new Vector2(6,-1)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject B_Queen_Prefab = Resources.Load<GameObject>("Prefabs/B_Queen_2D");
                    GameObject B_Queen = Instantiate(B_Queen_Prefab);
                    B_Queen.transform.position = chosenUnit.transform.position;
                    B_Queen.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, B_Queen);
                    break;
                default:
                    break;
            }
            PromotionBoardWhite.SetActive(false);
        }

        else
        {
            switch (clickPosition)
            {
                case Vector2 v when v.Equals(new Vector2(5, 0)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject W_Knight_Prefab = Resources.Load<GameObject>("Prefabs/W_Knight_2D");
                    GameObject W_Knight = Instantiate(W_Knight_Prefab);
                    W_Knight.transform.position = chosenUnit.transform.position;
                    W_Knight.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, W_Knight);
                    break;
                case Vector2 v when v.Equals(new Vector2(6, 0)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject W_Bishop_Prefab = Resources.Load<GameObject>("Prefabs/W_Bishop_2D");
                    GameObject W_Bishop = Instantiate(W_Bishop_Prefab);
                    W_Bishop.transform.position = chosenUnit.transform.position;
                    W_Bishop.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, W_Bishop);
                    break;
                case Vector2 v when v.Equals(new Vector2(5, -1)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject W_Rook_Prefab = Resources.Load<GameObject>("Prefabs/W_Rook_2D");
                    GameObject W_Rook = Instantiate(W_Rook_Prefab);
                    W_Rook.transform.position = chosenUnit.transform.position;
                    W_Rook.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, W_Rook);
                    break;
                case Vector2 v when v.Equals(new Vector2(6, -1)):
                    UnitMap.Remove(chosenUnitPosition);
                    chosenUnit.SetActive(false);
                    GameObject W_Queen_Prefab = Resources.Load<GameObject>("Prefabs/W_Queen_2D");
                    GameObject W_Queen = Instantiate(W_Queen_Prefab);
                    W_Queen.transform.position = chosenUnit.transform.position;
                    W_Queen.GetComponent<Base_Controller>().myPosition = chosenUnitPosition;
                    UnitMap.Add(chosenUnitPosition, W_Queen);
                    break;
                default:
                    break;
            }
            PromotionBoardWhite.SetActive(false);
        }


        promotion = false;
        TurnIsWhite = !TurnIsWhite;
        choosing = false;
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
        GameObject PromotionBoardBlack_Prefab = Resources.Load<GameObject>("Prefabs/PromotionBoardBlack");
        GameObject PromotionBoardWhite_Prefab = Resources.Load<GameObject>("Prefabs/PromotionBoardWhite");

        //B_Pawn
        for (int i = 0; i < 8; i++)
        {
            GameObject B_Pawn = Instantiate(B_Pawn_Prefab);
            B_Pawn.transform.position = new Vector2(-4 + i, -3+5*SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-4 + i, -3 + 5 * SWWV), B_Pawn);
            B_Pawn.GetComponent<Base_Controller>().myPosition = new Vector2(-4 + i, -3 + 5 * SWWV);
        }
        //B_Rook
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Rook = Instantiate(B_Rook_Prefab);
            B_Rook.transform.position = new Vector2(-4 + 7 * i, -4+7*SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-4 + 7 * i, -4+ 7 * SWWV), B_Rook);
            B_Rook.GetComponent<Base_Controller>().myPosition = new Vector2(-4 + 7*i, -4 + 7 * SWWV);
        }
        //B_Knight
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Knight = Instantiate(B_Knight_Prefab);
            B_Knight.transform.position = new Vector2(-3 + 5 * i, -4 + 7 * SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-3 + 5 * i, -4 + 7 * SWWV), B_Knight);
            B_Knight.GetComponent<Base_Controller>().myPosition = new Vector2(-3 + 5 * i, -4 + 7 * SWWV);
        }
        //B_Bishop
        for (int i = 0; i < 2; i++)
        {
            GameObject B_Bishop = Instantiate(B_Bishop_Prefab);
            B_Bishop.transform.position = new Vector2(-2 + 3 * i, -4 + 7 * SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-2 + 3 * i, -4 + 7 * SWWV), B_Bishop);
            B_Bishop.GetComponent<Base_Controller>().myPosition = new Vector2(-2 + 3 * i, -4 + 7 * SWWV);
        }
        //B_Queen
        GameObject B_Queen = Instantiate(B_Queen_Prefab);
        B_Queen.transform.position = new Vector2(0-SWWV, -4 + 7 * SWWV) * interval + offset;
        UnitMap.Add(new Vector2(0 - SWWV, -4 + 7 * SWWV), B_Queen);
        B_Queen.GetComponent<Base_Controller>().myPosition = new Vector2(0 - SWWV, -4 + 7 * SWWV);
        //B_King
        B_King = Instantiate(B_King_Prefab);
        B_King.transform.position = new Vector2(-1+SWWV, -4 + 7 * SWWV) * interval + offset;
        UnitMap.Add(new Vector2(-1 + SWWV, -4 + 7 * SWWV), B_King);
        B_King.GetComponent<Base_Controller>().myPosition = new Vector2(-1 + SWWV, -4 + 7 * SWWV);
        //W_Pawn
        for (int i = 0; i < 8; i++)
        {
            GameObject W_Pawn = Instantiate(W_Pawn_Prefab);
            W_Pawn.transform.position = new Vector2(-4 + i, 2-5*SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-4 + i, 2 - 5 * SWWV), W_Pawn);
            W_Pawn.GetComponent<Base_Controller>().myPosition = new Vector2(-4 + i, 2 - 5 * SWWV);
        }
        //W_Rook
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Rook = Instantiate(W_Rook_Prefab);
            W_Rook.transform.position = new Vector2(-4 + 7 * i, 3 - 7 * SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-4 + 7 * i, 3 - 7 * SWWV), W_Rook);
            W_Rook.GetComponent<Base_Controller>().myPosition = new Vector2(-4 + 7 * i, 3 - 7 * SWWV);
        }
        //W_Knight
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Knight = Instantiate(W_Knight_Prefab);
            W_Knight.transform.position = new Vector2(-3 + 5 * i, 3 - 7 * SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-3 + 5 * i, 3 - 7 * SWWV), W_Knight);
            W_Knight.GetComponent<Base_Controller>().myPosition = new Vector2(-3 + 5 * i, 3 - 7 * SWWV);
        }
        //W_Bishop
        for (int i = 0; i < 2; i++)
        {
            GameObject W_Bishop = Instantiate(W_Bishop_Prefab);
            W_Bishop.transform.position = new Vector2(-2 + 3 * i, 3 - 7 * SWWV) * interval + offset;
            UnitMap.Add(new Vector2(-2 + 3 * i, 3 - 7 * SWWV), W_Bishop);
            W_Bishop.GetComponent<Base_Controller>().myPosition = new Vector2(-2 + 3 * i, 3 - 7 * SWWV);
        }
        //W_Queen
        GameObject W_Queen = Instantiate(W_Queen_Prefab);
        W_Queen.transform.position = new Vector2(0 - SWWV, 3 - 7 * SWWV) * interval + offset;
        UnitMap.Add(new Vector2(0 - SWWV, 3 - 7 * SWWV), W_Queen);
        W_Queen.GetComponent<Base_Controller>().myPosition = new Vector2(0 - SWWV, 3 - 7 * SWWV);
        //W_King
        W_King = Instantiate(W_King_Prefab);
        W_King.transform.position = new Vector2(-1+SWWV, 3 - 7 * SWWV) * interval + offset;
        UnitMap.Add(new Vector2(-1 + SWWV, 3 - 7 * SWWV), W_King);
        W_King.GetComponent<Base_Controller>().myPosition = new Vector2(-1 + SWWV, 3 - 7 * SWWV);


        PromotionBoardBlack = Instantiate(PromotionBoardBlack_Prefab);
        PromotionBoardWhite = Instantiate(PromotionBoardWhite_Prefab);
        PromotionBoardBlack.SetActive(false);
        PromotionBoardWhite.SetActive(false);


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
