using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayButtonHandler : MonoBehaviour
{
    public Button yourButton;
    GameObject ResetButton;
    GameObject RotateButton;
    GameObject MatchingRequestButton;
    GameObject OtherPlayerText;

    void Start()
    {
        
        Button btn = yourButton.GetComponent<Button>();
        ResetButton = GameObject.Find("ResetButton").gameObject;
        RotateButton = GameObject.Find("RotateBoardButton").gameObject;
        MatchingRequestButton = GameObject.Find("MatchingReqeustButton").gameObject;
        OtherPlayerText = GameObject.Find("OtherPlayer").gameObject;
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        MatchingRequestButton.SetActive(false);
        OtherPlayerText.SetActive(false);
        ResetButton.SetActive(true);
        RotateButton.SetActive(true);
        GameObject.Find("UnitController").GetComponent<UnitController>().mode = UnitController.GameMode.SinglePlay;
        GameObject.Find("StartMenu").gameObject.SetActive(false);
    }
}
