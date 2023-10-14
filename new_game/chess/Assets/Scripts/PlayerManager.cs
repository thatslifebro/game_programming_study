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

    public void Add(S_PlayerList packet)
    {
        //Object obj = Resources.Load("Player");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject IdText_Prefab = Resources.Load<GameObject>("Prefabs/PlayerIdText");
            GameObject IdText = Object.Instantiate(IdText_Prefab) as GameObject;
            IdText.name = p.playerId.ToString();
            IdText.GetComponent<TMP_Text>().text = p.playerId.ToString();

            IdText.transform.SetParent(GameObject.Find("PlayerIdContent").transform);
            
        }
    }

    public void EnterGame(S_ResponseMatching packet)
    {
        GameObject otherPlayerText = GameObject.Find("OtherPlayer");
        otherPlayerText.GetComponent<TMP_Text>().text = $"You\n VS \n{packet.otherPlayerId}";
        otherPlayerText.GetComponent<TMP_Text>().color = Color.red;
        GameObject unitController = GameObject.Find("UnitController");
        unitController.GetComponent<UnitController>().StartWithWhiteView = packet.amIWhite;
        unitController.GetComponent<UnitController>().ResetGame();
        //if (packet.playerId == _myPlayer.PlayerId) return;
        //Object obj = Resources.Load("Player");
        //GameObject go = Object.Instantiate(obj) as GameObject;

        //Player player = go.AddComponent<Player>();
        //player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        //_players.Add(packet.playerId, player);
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
        //if (_myPlayer.PlayerId == packet.playerId)
        //{
        //    _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        //}
        //else
        //{
        //    Player player = null;
        //    if (_players.TryGetValue(packet.playerId, out player))
        //    {
        //        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        //    }
        //}
    }

}
