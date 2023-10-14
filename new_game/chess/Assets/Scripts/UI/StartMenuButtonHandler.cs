using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButtonHandler : MonoBehaviour
{
    public Button yourButton;

    GameObject StartMenu;
    
    void Start()
    {
        StartMenu = GameObject.Find("StartMenu").gameObject;
        
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameObject.Find("UnitController").GetComponent<UnitController>().mode = UnitController.GameMode.StartMenu;
        StartMenu.SetActive(true);
    }
}
