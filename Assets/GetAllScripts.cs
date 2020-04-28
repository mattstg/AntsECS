using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class GetAllScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        string[] files = Directory.GetFiles(Application.dataPath + "/SampleFolder","*.cs"); //REGEX
        foreach(string file in files)
        {
            Debug.Log("File: " + file);
        }

        string fileChosen = files[Random.Range(0, files.Length)];
        fileChosen = Path.GetFileNameWithoutExtension(fileChosen);
        Debug.Log("File Chosen: " + fileChosen);

        System.Type chosenType = Assembly.GetExecutingAssembly().GetType(fileChosen);
        
        Debug.Log("chosenType: " + chosenType);

        gameObject.AddComponent(chosenType);

        ////////


        var c = System.Activator.CreateInstance(typeof(MyClass));
        var d = System.Activator.CreateInstance(c.GetType(),new object[] { 5 });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MyClass
{
    public MyClass(int i) { }
}

