using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingRequestButtonHandler : MonoBehaviour
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
        C_RequestMatching packet = new C_RequestMatching();
        _network.Send(packet.Serialize());

    }
}
