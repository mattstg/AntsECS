using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ExposeAntMethodAttribute : Attribute
{
    public string attrDesc;

    public ExposeAntMethodAttribute(string attrDesc)
    {
        this.attrDesc = attrDesc;
    }
}
