using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateBoardButtonHandler : MonoBehaviour
{
    public Button yourButton;


    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("rotatebutton");
        GameObject.Find("UnitController").GetComponent<UnitController>().RotateBoard();
    }
}
