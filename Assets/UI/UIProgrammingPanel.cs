using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgrammingPanel : MonoBehaviour
{
    public InputField nameField;
    public Transform turingGroup;

    [HeaderAttribute("Resources")]
    public GameObject TuringSlotPrefab;


    public void LoadState(string name)
    {
        Debug.Log("Load State: " + name);
    }

    public void NameChanged(string newName)
    {
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
}
