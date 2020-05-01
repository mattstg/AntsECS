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
        funcDropDown.ClearOptions();
        funcDropDown.AddOptions(ReflectionManager.Instance.funcNames);
        funcDropDown.value = 0;
        DropdownChanged(0);
    }

    public void DeletePressed()
    {
        deleteFunc.Invoke();
    }

    public MethodPackage ExtractAsMethodPackage()
    {
        string name = funcDropDown.options[funcDropDown.value].text;
        MethodPackage toRet = ReflectionManager.Instance.methodPkgDict[name].CreateClone();
        List<object> args = new List<object>();
        foreach(UITuringArgSlot argslot in arguementsListPanel.GetComponentsInChildren<UITuringArgSlot>())
        {
            args.Add(argslot.GetValue());
        }
        toRet.args = args.ToArray();
        return toRet;


    }

    public void DropdownChanged(int newIndex)
    {
        //Redo the side options
        foreach(Transform t in arguementsListPanel)
        {
            GameObject.Destroy(t.gameObject);
        }

        MethodPackage mp = ReflectionManager.Instance.methodPkgDict[funcDropDown.options[newIndex].text];
        foreach (System.Type st in mp.argTypes)
        {
            GameObject go = GameObject.Instantiate(UIControl.instance.GenericTuringArguement);
            go.transform.SetParent(arguementsListPanel);
            go.GetComponent<UITuringArgSlot>().InitializeArgSlot(mp.methodName, st);
            UIControl.instance.attrDesc.text = string.Format("<b>{0}</b> \n {1}", mp.methodName, ((ExposeAntMethodAttribute)(mp.mi.GetCustomAttributes(typeof(ExposeAntMethodAttribute), false)[0])).attrDesc);
        }

    }
}
