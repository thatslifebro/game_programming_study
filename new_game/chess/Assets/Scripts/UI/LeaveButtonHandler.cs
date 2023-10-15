using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaveButtonHandler : MonoBehaviour
{
    public Button yourButton;
    NetworkManager _network;
    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    } 

    void TaskOnClick()
    {
        C_LeaveGame packet = new C_LeaveGame();
        _network.Send(packet.Serialize());
        GameObject.Find("OtherPlayer").GetComponent<TMP_Text>().text = "You \nvs";
    }
}
