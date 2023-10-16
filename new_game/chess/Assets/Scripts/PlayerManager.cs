using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static S_PlayerList;
using TMPro;

public class PlayerManager
{
    // Start is called before the first frame update
    //MyPlayer _myPlayer;
    //Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static PlayerManager Instance { get; } = new PlayerManager();

    public int myId;

    public void Add(S_PlayerList packet)
    {
        //Object obj = Resources.Load("Player");
        GameObject PlayerIdContent = GameObject.Find("PlayerIdContent");
        Transform[] children = PlayerIdContent.GetComponentsInChildren<Transform>();
        if(children != null)
        {
            for(int i = 2; i < children.Length; i++)
            {
                if (children[i] != PlayerIdContent.transform)
                {
                    Object.Destroy(children[i].gameObject);
                }
            }
        }
        GameObject.Find("Title").GetComponent<TMP_Text>().text = $"Online Player List ({packet.players.Count})";
        GameObject MyText = null;
        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject IdText_Prefab = Resources.Load<GameObject>("Prefabs/PlayerIdText");
            GameObject IdText = Object.Instantiate(IdText_Prefab) as GameObject;
            IdText.name = p.playerId.ToString();
            if (p.isInGame)
            {
                IdText.GetComponent<TMP_Text>().text = $"{ p.playerId.ToString()}  - In Game";
            }
            else
            {
                IdText.GetComponent<TMP_Text>().text = p.playerId.ToString();
            }
            
            if (p.isSelf)
            {
                myId = p.playerId;
                IdText.GetComponent<TMP_Text>().color = Color.green;
                MyText = IdText;
            }
            
            IdText.transform.SetParent(GameObject.Find("PlayerIdContent").transform,false);

        }
        if(MyText)
            MyText.transform.SetSiblingIndex(1);
    }

    public void EnterGame(S_ResponseMatching packet)
    {
        GameObject otherPlayerText = GameObject.Find("OtherPlayer");
        
        otherPlayerText.GetComponent<TMP_Text>().text = $"You\n VS \n  {packet.otherPlayerId}";
        otherPlayerText.GetComponent<TMP_Text>().color = Color.red;
        GameObject unitController = GameObject.Find("UnitController");
        unitController.GetComponent<UnitController>().ResetGame(packet.amIWhite);
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        //if (_myPlayer.PlayerId == packet.playerId)
        //{
        //    GameObject.Destroy(_myPlayer.gameObject);
        //    _myPlayer = null;
        //} else
        //{
        //    Player player = null;
        //    if(_players.TryGetValue(packet.playerId, out player))
        //    {
        //        GameObject.Destroy(player.gameObject);
        //        _players.Remove(packet.playerId);
        //    }
        //}
    }

    public void Move(S_BroadcastMove packet)
    {
        if (packet.playerId == myId) return;

        Vector2 prev = new Vector2(packet.prevX, packet.prevY);
        Vector2 next = new Vector2(packet.nextX, packet.nextY);
        GameObject.Find("UnitController").GetComponent<UnitController>().OtherPlayerMove(prev,next,packet.promotion);
    }

    public void GameOver(S_GameOver packet)
    {
        GameObject GameResult = GameObject.Find("GameResult");
        if (packet.Draw)
            GameResult.GetComponent<TMP_Text>().text = "Draw";
        else if (packet.youWin)
            GameResult.GetComponent<TMP_Text>().text = "You Win!";
        else
            GameResult.GetComponent<TMP_Text>().text = "You Lose!";
    }

}
