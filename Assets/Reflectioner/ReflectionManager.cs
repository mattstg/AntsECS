using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class ReflectionManager
{
    #region singleton
    private static ReflectionManager instance;
    public static ReflectionManager Instance { get { return instance ?? (instance = new ReflectionManager()); } }
    private ReflectionManager() { }
    #endregion

    public Dictionary<string, MethodPackage> methodPkgDict { get; private set; }
    public List<string> funcNames;

    public void CreateAllMethodPackages()
    {
        //Step one, extract all methods from Ant.cs which have the correct attribute
        methodPkgDict = new Dictionary<string, MethodPackage>();

        MethodInfo[] mis = typeof(Ant).GetAllMethodsWithAttribute<ExposeAntMethodAttribute>();
        //Then fill dictionary
        mis.ForEach((kv) =>
        {
            methodPkgDict.Add(kv.Name, new MethodPackage(kv));
        });

        funcNames = methodPkgDict.Keys.ToList();
    }
}
