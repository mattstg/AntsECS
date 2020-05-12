using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStateDictionary 
{
    #region singleton
    private static MasterStateDictionary instance;
    public static MasterStateDictionary Instance { get { return instance ?? (instance = new MasterStateDictionary()); } }
    private MasterStateDictionary() { masterStateDict = new Dictionary<string, Dictionary<int,List<MethodPackage>>>(); }
    #endregion

    public Dictionary<string, Dictionary<int,List<MethodPackage>>> masterStateDict { get; private set; }

    public void SaveCurrentState(Dictionary<string, Dictionary<int, List<MethodPackage>>> toSave)
    {
        masterStateDict = new Dictionary<string, Dictionary<int, List<MethodPackage>>>();
        foreach (KeyValuePair<string, Dictionary<int, List<MethodPackage>>> kv in toSave)
        {
            masterStateDict.Add(kv.Key, new Dictionary<int, List<MethodPackage>>());
            foreach (KeyValuePair<int, List<MethodPackage>> subDict in kv.Value)
            {
                masterStateDict[kv.Key].Add(subDict.Key, new List<MethodPackage>());
                foreach (MethodPackage mp in subDict.Value)
                {
                    masterStateDict[kv.Key][subDict.Key].Add(mp.CreateClone());
                }
            }
        }
    }
}
