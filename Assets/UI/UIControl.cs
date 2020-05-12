using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIControl : MonoBehaviour
{
    public static UIControl instance; //lazy singleton

    public GameObject MainTuringPanel;
    public GameObject HUD;
    public GameObject StatePanel;
    public GameObject inputPanel;    
    public Text attrDesc;
    public InputField nameField;
    public InputField inputNameField;
    public Button specialAddInputBtn;
    public Button specialAddStateBtn;

    [Header("Resources")]
    public GameObject StateButton;
    public GameObject GenericTuringArguement;
    public GameObject InputBtn;
    public GameObject TuringResourcePanel;
    public GameObject TuringSlotPrefab;

    [HideInInspector] public Button selectedStateBtn;
    [HideInInspector] public Button selectedInputStateBtn;
    int selectedInputState;
    string selectedState;

    public Dictionary<string, Dictionary<int, List<MethodPackage>>> localMasterDictCopy { get; private set; } //used in UI, to be saved into MasterStateDict

    public void Initialize()
    {
        instance = this;
        //Need to do a deep clone
        localMasterDictCopy = new Dictionary<string, Dictionary<int, List<MethodPackage>>>();
        foreach(KeyValuePair<string, Dictionary<int, List<MethodPackage>>> kv in MasterStateDictionary.Instance.masterStateDict)
        {
            localMasterDictCopy.Add(kv.Key, new Dictionary<int, List<MethodPackage>>());
            foreach(KeyValuePair<int, List<MethodPackage>> subDict in kv.Value)
            {
                localMasterDictCopy[kv.Key].Add(subDict.Key, new List<MethodPackage>());
                foreach(MethodPackage mp in subDict.Value)
                {
                    localMasterDictCopy[kv.Key][subDict.Key].Add(mp.CreateClone());
                }
            }
        }


        //if its empty, create a default one
        if(localMasterDictCopy.Keys.Count <= 0)
        {
            localMasterDictCopy.Add("Queen", new Dictionary<int, List<MethodPackage>>() { { 0, new List<MethodPackage>() } });
            localMasterDictCopy.Add("Worker", new Dictionary<int, List<MethodPackage>>() { { 0, new List<MethodPackage>() } });
            localMasterDictCopy.Add("Nurse", new Dictionary<int, List<MethodPackage>>() { { 0, new List<MethodPackage>() } });
        }


        selectedState = localMasterDictCopy.Keys.ToArray()[0];
        selectedInputState = localMasterDictCopy[selectedState].Keys.ToArray()[0];
        DrawMainPanel();
        DrawSidePanel();
        DrawStateSelectionPanel();
    }
    
    public void FlowUpdate(float dt)
    {

    }

    public void ToggleTuringPanel(bool open)
    {
        MainTuringPanel.SetActive(open);
        HUD.SetActive(!open);
    }

    public void AddInputBtnPressed()
    {
        GameObject newState = GameObject.Instantiate(InputBtn);
        newState.transform.SetParent(inputPanel.transform);
        Button b = newState.GetComponent<Button>();
        newState.name = GetNextAvailableInputNumber().ToString();
        b.GetComponentInChildren<Text>().text = newState.name;
        b.onClick.AddListener(() => InputPressed(newState.name, b)); //Want name dynamic, so make function that passes name
        localMasterDictCopy[selectedState].Add(int.Parse(newState.name), new List<MethodPackage>());
    }

    public int GetNextAvailableInputNumber()
    {
        for(int i = 0; i < 99999; i++)
        {
            if (!localMasterDictCopy[selectedState].ContainsKey(i))
                return i;
        }
        Debug.LogError("How was this possibly full? returning -1");
        return -1;
    }

    public string GetNextAvailableState()
    {
        string[] randomizedArrayOfStateNames = new string[] { "Queen", "Worker", "Nurse", "Road", "Feeder", "Alice", "Bob", "Charlie", "Doggo", "Edward", "Forenzo", "Gilbert", "Homer", "Igor", "J", "k", "l", "M", "N", "O", "P", "Q", "R" };
        for (int i = 0; i < randomizedArrayOfStateNames.Length; i++)
        {
            if (!localMasterDictCopy.ContainsKey(randomizedArrayOfStateNames[i]))
                return randomizedArrayOfStateNames[i];
        }
        Debug.LogError("Took too many default names, chose your own names!");
        return "Error";
    }

    public void AddStatePressed()
    {
        GameObject newState = GameObject.Instantiate(StateButton);
        newState.transform.SetParent(StatePanel.transform);
        string newName = GetNextAvailableState();
        newState.name = newName;
        Button b = newState.GetComponent<Button>();
        b.GetComponentInChildren<Text>().text = newState.name;
        b.onClick.AddListener(() => StatePressed(newState.name,b)); //Want name dynamic, so make function that passes name
        localMasterDictCopy.Add(newState.name, new Dictionary<int, List<MethodPackage>>());
    }

    public void AddSlotPressed()
    {
        GameObject go = GameObject.Instantiate(TuringSlotPrefab);
        go.transform.SetParent(MainTuringPanel.transform);
        Debug.Log("Forgot to add to dict");
    }

    public void InputPressed(string inputText, Button b)
    {
        selectedInputState = int.Parse(inputText);
        DrawMainPanel();
    }

    public void StatePressed(string stateName, Button b)
    {
        selectedState = stateName;
        var v = localMasterDictCopy[selectedState].Keys.ToArray();
        if (v.Length > 0)
            selectedInputState = v[0];
        else
            selectedInputState = -1;

        DrawMainPanel();
        DrawSidePanel();
    }

    public void SavePressed()
    {
        MasterStateDictionary.Instance.SaveCurrentState(localMasterDictCopy);
    }

    public void SelectedInputNameChanged(string newName)
    {
        int oldName = selectedInputState;
        selectedInputState = int.Parse(newName);

        localMasterDictCopy[selectedState].Add(selectedInputState, localMasterDictCopy[selectedState][oldName]);
        localMasterDictCopy[selectedState].Remove(oldName);
    }

    public void SelectedStateNameChanged(string newName)
    {
        string oldName = selectedState;
        selectedState = newName;
        localMasterDictCopy.Add(newName, localMasterDictCopy[oldName]);
        localMasterDictCopy.Remove(oldName);
    }

    public void DrawSidePanel()
    {
        Debug.Log("Draw inputState sides: " + selectedInputState);
        //Using selected state and input
        foreach (Transform t in inputPanel.transform)
        {
            if(t != specialAddInputBtn.transform)
                GameObject.Destroy(t.gameObject);
        }


        foreach(var kv in localMasterDictCopy[selectedState])
        {
            GameObject newState = GameObject.Instantiate(InputBtn);
            newState.transform.SetParent(inputPanel.transform);
            newState.name = kv.Key.ToString();
            Button b = newState.GetComponent<Button>();
            b.GetComponentInChildren<Text>().text = newState.name;
            b.onClick.AddListener(() => InputPressed(newState.name, b)); //Want name dynamic, so make function that passes name
        }        
    }

    public void DrawMainPanel()
    {
        Debug.Log("Draw for State: " + selectedState + " and inputState: " + selectedInputState);
        //Using selected state and input
        foreach(Transform t in MainTuringPanel.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        var v = localMasterDictCopy[selectedState];
        var vv = v.ContainsKey(selectedInputState);

        if (localMasterDictCopy[selectedState].ContainsKey(selectedInputState))
            foreach (MethodPackage mp in localMasterDictCopy[selectedState][selectedInputState])
            {
                GameObject turingSlot = GameObject.Instantiate(TuringSlotPrefab);
                turingSlot.GetComponent<UITuringSlot>().Initialize(mp.methodName, () => { SlotDeleted(turingSlot); });
            }
        
    }

    public void DrawStateSelectionPanel()
    {
        Debug.Log("Draw inputState sides: " + selectedInputState);
        //Using selected state and input
        foreach (Transform t in StatePanel.transform)
        {
            if (t != specialAddStateBtn.transform)
                GameObject.Destroy(t.gameObject);
        }


        foreach (var kv in localMasterDictCopy)
        {

            GameObject newState = GameObject.Instantiate(StateButton);
            newState.transform.SetParent(StatePanel.transform);
            newState.name = kv.Key;
            Button b = newState.GetComponent<Button>();
            b.GetComponentInChildren<Text>().text = newState.name;
            b.onClick.AddListener(() => StatePressed(newState.name, b)); //Want name dynamic, so make function that passes name
        }
    }

    public void SlotDeleted(GameObject toDelete)
    {

        GameObject.Destroy(toDelete);
    }

}

