using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

[System.Serializable]
public class MethodPackage
{
    public string methodName;
    public MethodInfo mi;
    public object[] args;     //Filled outside constructor
    public Type[] argTypes;

    public MethodPackage(MethodInfo mi)
    {
        this.mi = mi;

        methodName = mi.Name;
        ParameterInfo[] pis = mi.GetParameters();
        List<Type> tempTypes = new List<Type>();
        foreach(ParameterInfo pi in pis)
        {
            tempTypes.Add(pi.ParameterType);
        }
        argTypes = tempTypes.ToArray();
    }

    public MethodPackage CreateClone()
    {
        MethodPackage toRet = new MethodPackage(mi);
        toRet.args = args;
        return toRet;
    }
}
