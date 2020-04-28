using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject MainTuringPanel;
    public GameObject HUD;
    public GameObject StatePanel;
    public UIProgrammingPanel programmingPanel;

    [Header("Resources")]
    public GameObject StateButton;

    public void ToggleTuringPanel(bool open)
    {
        MainTuringPanel.SetActive(open);
        HUD.SetActive(!open);
    }

    public void AddStatePressed()
    {
        GameObject newState = GameObject.Instantiate(StateButton);
        newState.transform.SetParent(StatePanel.transform);
        newState.GetComponent<Button>().onClick.AddListener(() => StatePressed(newState.name)); //Want name dynamic, so make function that passes name
    }

    public void StatePressed(string stateName)
    {
        programmingPanel.LoadState(stateName);
    }

}
