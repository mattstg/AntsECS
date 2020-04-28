using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITuringSlot : MonoBehaviour
{
    public Dropdown funcDropDown;
    public Transform arguementsListPanel;
    public System.Action deleteFunc;

    public void Initialize(string funcName, System.Action _deleteFunc)
    {
        deleteFunc = _deleteFunc;
    }

    public void DeletePressed()
    {
        deleteFunc.Invoke();
    }


    public void DropdownChanged(int newIndex)
    {

    }
}
