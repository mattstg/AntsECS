using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgrammingPanel : MonoBehaviour
{
    string selectedState;
    public InputField nameField;
    public Transform turingGroup;
    MethodPackage currentMethodPackage;

    [HeaderAttribute("Resources")]
    public GameObject TuringSlotPrefab;


    public void LoadState(string name)
    {
        selectedState = name;
        Debug.Log("Load State: " + name);
    }

    public void NameChanged(string newName)
    {
        UIControl.instance.StateButton.name = newName;
        Debug.Log("NameChanged");
    }

    public void AddSlot()
    {
        GameObject go = GameObject.Instantiate(TuringSlotPrefab);
        go.transform.SetParent(turingGroup);
        UITuringSlot slot = go.GetComponent<UITuringSlot>();
        slot.Initialize("", () => SlotDeleted(slot));
    }

    public void SlotDeleted(UITuringSlot toDelete)
    {
        Debug.Log("Slot delete");
    }

    public void SavePressed()
    {
        List<MethodPackage> methodPackages = new List<MethodPackage>();
        foreach(Transform t in turingGroup)
        {
            UITuringSlot uslot = t.GetComponent<UITuringSlot>();
            methodPackages.Add(uslot.ExtractAsMethodPackage());
        }
        MasterStateDictionary.Instance.SaveCurrentState(selectedState, methodPackages);
    }
}
