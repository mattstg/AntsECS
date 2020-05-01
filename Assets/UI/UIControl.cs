using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public static UIControl instance; //lazy singleton

    public GameObject MainTuringPanel;
    public GameObject HUD;
    public GameObject StatePanel;
    public UIProgrammingPanel programmingPanel;
    public Text attrDesc;

    [Header("Resources")]
    public GameObject StateButton;
    public GameObject GenericTuringArguement;
    
    [HideInInspector]
    public Button selectedState;

    public void Initialize()
    {
        instance = this;
    }
    
    public void FlowUpdate(float dt)
    {

    }

    public void ToggleTuringPanel(bool open)
    {
        MainTuringPanel.SetActive(open);
        HUD.SetActive(!open);
    }

    public void AddStatePressed()
    {
        GameObject newState = GameObject.Instantiate(StateButton);
        newState.transform.SetParent(StatePanel.transform);
        Button b = newState.GetComponent<Button>();
        b.onClick.AddListener(() => StatePressed(newState.name,b)); //Want name dynamic, so make function that passes name
    }

    public void StatePressed(string stateName, Button b)
    {
        programmingPanel.LoadState(stateName);
        selectedState = b;
    }

    public void SavePressed()
    {
        programmingPanel.SavePressed();
    }

}
