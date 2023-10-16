using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayButtonHandler : MonoBehaviour
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
        MatchingRequestButton.SetActive(true);
        OtherPlayerText.SetActive(true);
        ResetButton.SetActive(false);
        RotateButton.SetActive(false);
        GameObject.Find("UnitController").GetComponent<UnitController>().mode = UnitController.GameMode.MultiPlay;
        GameObject.Find("StartMenu").gameObject.SetActive(false);


    }
}
