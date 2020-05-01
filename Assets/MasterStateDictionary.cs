using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStateDictionary 
{
    #region singleton
    private static MasterStateDictionary instance;
    public static MasterStateDictionary Instance { get { return instance ?? (instance = new MasterStateDictionary()); } }
    private MasterStateDictionary() { masterStateDict = new Dictionary<string, List<MethodPackage>>(); }
    #endregion

    public Dictionary<string, List<MethodPackage>> masterStateDict { get; private set; }

    public void SaveCurrentState(string stateName, List<MethodPackage> methodPkgs)
    {

    }
}
