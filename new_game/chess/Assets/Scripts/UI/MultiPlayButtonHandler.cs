using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayButtonHandler : MonoBehaviour
{
    public Button yourButton;
    GameObject ResetButton;
    GameObject RotateButton;
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        ResetButton = GameObject.Find("ResetButton").gameObject;
        RotateButton = GameObject.Find("RotateBoardButton").gameObject;
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        ResetButton.SetActive(false);
        RotateButton.SetActive(false);
        GameObject.Find("UnitController").GetComponent<UnitController>().mode = UnitController.GameMode.MultiPlay;
        GameObject.Find("StartMenu").gameObject.SetActive(false);


    }
}
