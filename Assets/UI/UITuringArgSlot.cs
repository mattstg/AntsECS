using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITuringArgSlot : MonoBehaviour
{
    public enum ArgType { _int, _float, _enum }

    public Text labelText;
    public InputField inputField;
    public Dropdown dropdownField;
    ArgType argType;
    System.Type sysType;

    public void InitializeArgSlot(string methodName, System.Type st)
    {
        sysType = st;
        if (st == typeof(int))
        {
            argType = ArgType._int;
            inputField.gameObject.SetActive(true);
            inputField.contentType = InputField.ContentType.IntegerNumber;
        }
        else if (st == typeof(float))
        {
            argType = ArgType._float;
            inputField.gameObject.SetActive(true);
            inputField.contentType = InputField.ContentType.DecimalNumber;
        }
        else if (st.IsEnum)
        {
            argType = ArgType._enum;
            dropdownField.gameObject.SetActive(true);
            dropdownField.ClearOptions();
            dropdownField.AddOptions(st.GetStringListOfEnums());
        }
        else
        {
            Debug.LogError("Type: " + st.ToString() + " is not yet available, add it to DropdownChange in UITuringSlot");
        }
        labelText.text = methodName;
    }

    public object GetValue()
    {
        switch (argType)
        {
            case ArgType._int:
                return (object)inputField.text;
            case ArgType._float:
                return (object)inputField.text;
            case ArgType._enum:
                return System.Enum.Parse(sysType, dropdownField.options[dropdownField.value].text);  //System.Convert.ChangeType(dropdownField.value, sysType);
                                                                                                    //(object)dropdownField.options[dropdownField.value].text;
            default:
                Debug.LogError("Unhandled Switch: " + argType);
                return null;
                
        }

    }
}
