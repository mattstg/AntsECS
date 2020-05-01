using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GV
{
    public static string[] GetStringArrOfEnums<T>() where T : struct, System.IConvertible
    {
        return ConvertArrToString<T>(GetArrOfEnums<T>()).ToArray();
    }

    public static List<string> GetStringListOfEnums<T>() where T : struct, System.IConvertible
    {
        return ConvertArrToString<T>(GetArrOfEnums<T>()).ToList();
    }

    static Dictionary<System.Type, System.Enum[]> bakedEnumDict = new Dictionary<System.Type, System.Enum[]>();
    public static T[] GetArrOfEnums<T>() where T : struct, System.IConvertible
    {
        //Not sure if micro opt, but bakes the enums in a dict first time retrieval, since we retreieve some of them alot of times
        if (!bakedEnumDict.ContainsKey(typeof(T)))
            bakedEnumDict.Add(typeof(T), System.Enum.GetValues(typeof(T)).Cast<System.Enum>().ToArray<System.Enum>());

        return bakedEnumDict[typeof(T)].Cast<T>().ToArray<T>();
    }

    public static string[] ConvertArrToString<T>(T[] arr)
    {
        string[] toRet = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
            toRet[i] = arr[i].ToString();
        return toRet;
    }

}
