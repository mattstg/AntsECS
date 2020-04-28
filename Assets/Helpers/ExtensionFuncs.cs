using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public static class ExtensionFuncs  {

    static System.Random rnd = new System.Random();

    public static T GetCopyOf<T>(this GameObject go, T other) where T : Component
    {
        T myComponent = go.AddComponent<T>();
        myComponent.GetCopyOf(other);
        return myComponent;
        //
        //System.Type myType = typeof(T);
        //Component c = go.AddComponent(myType);

    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            //MethodInfo mi = pinfo.GetSetMethod();
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch (Exception e)
                {
                    Debug.LogError("GetCopyOfFailed: " + e.ToString());

                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }


    public static T AddCopyComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }


    public static T GetRandomElement<T>(this System.Collections.Generic.ICollection<T> iCollec)
    {
        if (iCollec.Count <= 0)
            return default(T);
        return iCollec.ElementAt(UnityEngine.Random.Range(0, iCollec.Count));
    }

    public static int Sign(this bool b, bool positiveIsNegative = false)
    {
        if(!positiveIsNegative)
            return (b) ? 1 : -1;
        else
            return (b) ? -1 : 1;

    }

    public static void DeleteAllChildren(this Transform t)
	{
		Transform[] childarr = t.ChildrenArray();
		foreach (Transform child in childarr)
		{
			GameObject.Destroy(child.gameObject);
		}
	}

	public static T ToEnum<T>(this string s)
		where T : struct, IConvertible, IComparable, IFormattable
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("String conversion to enum failed, T was not enum type, must be an enum.");
		}
		T toRet;
		if (Enum.TryParse<T>(s, out toRet))
		{
			return toRet;
		}
		else
		{
			System.Type st = typeof(T);
			Debug.LogError($"ERROR, Conversion from string to enum for string:{s} and enumType:{st} failed!! Returning default value for enum");
			return default(T);
		}

	}

    
	public static Transform[] ChildrenArray(this Transform t)
	{
		return t.Cast<Transform>().ToArray();
	}

	public static Transform ChildByName(this Transform t, string _name)
	{
		return t.ChildrenArray().FirstOrDefault(tchild => tchild.name == _name);
	}

	public static float SurfaceArea(this Vector2 v)
	{
		return v.x * v.y;
	}

	public static Vector3 V3(this Vector2 v)
	{
		return new Vector3(v.x, v.y,0);
	}
    
	public static Vector3 Abs(this Vector3 v)
	{
		return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
	}

	public static Vector2 Mult(this Vector2 v, Vector2 multBy)
	{
		return new Vector2(v.x * multBy.x, v.y * multBy.y);
	}

	public static T[] GetAllCompRecursive<T>(this Transform t) where T : Component
	{
		List<T> toFill = new List<T>();
		_GetAllCompRecursive<T>(t, toFill);
		return toFill.ToArray();
	}

	private static void _GetAllCompRecursive<T>(this Transform t, List<T> allComp) where T : Component
    {
		if (t.childCount > 0)
			foreach (Transform child in t)
				_GetAllCompRecursive<T>(child, allComp);
		T compToAdd = t.GetComponent<T>();
		if(compToAdd)
			allComp.Add(compToAdd);
	}


    public static float Random(this Vector2 v)
    {
        return UnityEngine.Random.Range(v.x, v.y);
    }

    public static bool IsBetween(this Vector2 v, float v2)
    {
        return v2 >= v.x && v2 <= v.y;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        //if (source == null) throw new ArgumentNullException("source");
        //if (action == null) throw new ArgumentNullException("action");

        foreach (T item in source)
            action(item);
    }

    public static bool TrueForAll<T>(this IEnumerable<T> source, Predicate<T> action)
    {
        foreach (T item in source)
            if (!action(item))
                return false;
        return true;
    }

    public delegate G ParsingDelg<T,G>(T elem);
    public static G[] CollectionFromForEach<T,G>(this IEnumerable<T> source, ParsingDelg<T,G> parsingDelg)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (parsingDelg == null) throw new ArgumentNullException("action");

        List<G> toRet = new List<G>();

        foreach (T item in source)
        {
            toRet.Add(parsingDelg(item));
        }
        return toRet.ToArray();
    }

    public delegate bool conditionalDelg<T>(T element);
	//public Predicate( )

	public static List<T> GetAll<T>(this IEnumerable<T> source, Predicate<T> action)
	{
		List<T> toReturn = new List<T>(); 
		foreach(T t in source)
		{
			if (action.Invoke(t))
				toReturn.Add(t);
		}
		return toReturn;
	}

    public static bool IsMaskContainedInThisLayerMask(this int lm, string layerName)
    {
        int n = LayerMask.NameToLayer(layerName);
        return lm == (lm | (1 << n)); //self explainatory
    }

    public static bool IsMaskContainedInThisLayerMask(this int lm, int layer)
    {
        return lm == (lm | (1 << layer)); //see above
    }

    

    public static T GetRandom<T>(this T[] v)
    {
        if (v.Length <= 0)
        {
            Debug.LogError("GetRandom was given an array of length less than 0");
            return default(T);
        }
        return v[UnityEngine.Random.Range(0, v.Length)];
    }

    public static List<T> RandomizeOrder<T>(this List<T> v)
    {
        var result = v.OrderBy(item => rnd.Next());
        return result.ToList();
    }

   
    /// <summary>
    /// EX) Given a percent range of .5 and a value of 10 will return a value between 5 and 15, Given zero, it is the same value
    /// </summary>
    /// <param name="v"></param>
    /// <param name="withinPercentRange"></param>
    /// <returns></returns>
    public static float RandomizeByPercent(this float v, float withinPercentRange)
    {
        if (withinPercentRange == 0)
            return v;
        return v + UnityEngine.Random.Range(-v * withinPercentRange, v * withinPercentRange);
    }

	public static string[] CollectionToStringArray<T>(this System.Collections.Generic.ICollection<T> v)
	{
		string[] toRet = new string[v.Count];
		int i = 0; //cannot use for, for this situtation
		foreach(T elem in v)
		{
			toRet[i] = elem.ToString();
			i++;
		}
		return toRet;
	}

    

    public static string CollectionToString<T>(this System.Collections.Generic.ICollection<T> v)
    {
        return StringArrayToString(CollectionToStringArray<T>(v));
    }


    public static string StringArrayToString(this string[] v)
    {
        string toRet = "";
        foreach (string elem in v)
        {
            toRet += elem + ", ";
        }

        if (string.IsNullOrEmpty(toRet))
            return toRet;
        return toRet.Substring(0, toRet.Length - 2); //remove last ", "
    }

    public static Vector2 AngToV2(this float v)
    {
        return DegreeToVector2(v);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }
    //JointLimitState2D
}
